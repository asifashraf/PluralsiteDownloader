﻿(function () {
    'use strict';

    angular.module('app')
    .controller('courseCtrl', courseCtrl);

    courseCtrl.$inject = ['$scope', '$rootScope', 'coursesService', '_', 'Hub', '$timeout', 'toaster'];

    function courseCtrl($scope, $rootScope, coursesService, _, Hub, $timeout, toaster) {
        var vm = this;
        vm.courseName = '';
        vm.course = undefined;
        vm.clipsToDownloadQueue = [];
        vm.currentlyDownloading = false;
        vm.toggleModuleAccordion = toggleModuleAccordion;
        vm.loadCourseData = loadCourseData;
        vm.addClipToDownloadList = addClipToDownloadList;
        vm.addModuleToDownloadList = addModuleToDownloadList;
        vm.addCourseToDownloadList = addCourseToDownloadList;
        vm.processClipsQueue = processClipsQueue;
        vm.downloadClip = downloadClip;
        vm.delayBetweenClips = 20; // delay in seconds

        activate();
        /////////////////////////////////////

        function activate() {
            //declaring the hub connection
            var hub = new Hub('ProgressHub', {
                //client side methods
                listeners: {
                    'updateProgress': function (progress) {
                        var requiredClip = _.get(vm.course, '.courseModules[' + progress.extra.moduleIndex + ']'
                            + '.clips[' + progress.extra.clipIndex + ']');

                        $timeout(function () {
                            if (requiredClip) {
                                requiredClip.progress = progress;
                            }
                        }, 0);
                    }
                }
            });

            // register events' listeners
            $scope.$on('clipsToDownloadQueue.push', function () {
                vm.processClipsQueue();
            });
            $scope.$on('clipsToDownloadQueue.finish', function () {
                vm.processClipsQueue();
            });
        }

        function toggleModuleAccordion(module) {
            module.isAccordionOpen = !module.isAccordionOpen;
        }

        function loadCourseData() {
            var title = vm.courseName.replace(/\s/g, '');
            if (vm.courseName.indexOf('/') > 0) {
                title = _.last(vm.courseName.split('/'));
            }
            return coursesService.getCourseData(title).then(function (course) {
                if (course.title == null) {
                    alert('Course not found!.');
                    vm.course = undefined;
                } else {
                    vm.course = course;
                }
            }, function (error) {
                toaster.pop({
                    type: 'error',
                    title: '',
                    body: 'Couldn\'t retrieve course data. Please make sure that you have the correct course name and try again.'
                });
            });
        }

        function addClipToDownloadList(clip, module) {
            var clipFound = _.find(vm.clipsToDownloadQueue, function (item) {
                return item.clip.name === clip.name;
            });
            if (!clipFound) {
                vm.clipsToDownloadQueue.push({ clip: clip, module: module });
                clip.progress.isDownloading = true; // disable download button
                // fire an event notifying that a clip has been added to downloads queue so that the controller starts to download it.
                $scope.$emit("clipsToDownloadQueue.push");
            }
        }

        function addModuleToDownloadList(module, $event) {
            _.forEach(module.clips, function (clip) {
                vm.addClipToDownloadList(clip, module);
            });
            if ($event && module.isAccordionOpen) {
                $event.preventDefault();
                $event.stopPropagation();
            }
        }

        function addCourseToDownloadList() {
            _.forEach(vm.course.courseModules, function (module) {
                module.isAccordionOpen = true;
                vm.addModuleToDownloadList(module);
            });
        }
        var ll = console.log;
        function processClipsQueue() {
            // make sure that controller is NOT downloading any clips in the mean time.
            if (vm.clipsToDownloadQueue.length > 0 && !vm.currentlyDownloading) {
                var nextItemToDownload = vm.clipsToDownloadQueue.shift();
                vm.currentlyDownloading = true;
                //var duration = toSeconds(nextItemToDownload.clip.duration);
                var hold = toSeconds(nextItemToDownload.clip.hold);
                if (hold > 150) {
                    hold = 150;
                }
                if (hold < 20 && hold > 0) {
                    hold = 20;
                }
                ll("holding for: ", hold + " secs");
                $timeout(function () {

                    
                    ll("Downloading: ", nextItemToDownload.clip.title + "-" + nextItemToDownload.clip.duration);
                    //delay here
                    return vm.downloadClip(nextItemToDownload.clip, nextItemToDownload.module);
                }, hold * 1000); // 120 seconds delay to avoid blocking account.
            }
        }
        function toSeconds(hms) {
            //ll("hms", hms);
            var a = hms.toString().split(':'); // split it at the colons

            // minutes are worth 60 seconds. Hours are worth 60 minutes.
            var sec = (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
            //ll("seconds", sec/3);

            return window.parseInt(Math.ceil(sec / 3));
            
        }
        function downloadClip(clip, module) {
            clip.progress.isDownloading = true;
            toaster.pop({
                type: 'info',
                title: '',
                body: 'Starting saving video <b>"' + clip.title + '</b>".',
                bodyOutputType: 'trustedHtml'
            });
            clip.courseTitle = vm.course.title;
            clip.moduleTitle = module.title;
            clip.moduleIndex = _.findIndex(vm.course.courseModules, function (moduleItem) {
                return moduleItem.title === module.title;
            });
            return coursesService.downloadCourseModuleClip(clip).then(function (progress) {
                toaster.pop({
                    type: 'success',
                    title: '',
                    body: 'Video file <b>"' + progress.fileName + '</b>" saved.',
                    bodyOutputType: 'trustedHtml'
                });
                $timeout(function () {
                    clip.progress.hasBeenDownloaded = true;
                    clip.progress.isDownloading = false;
                }, 500); // sometimes, progress callback comes after success callbak.
            }, function (errorResponse) {
                switch (errorResponse.status) {
                    case 429:
                        var downloadDelaySec = _.random(5, 15);
                        toaster.pop({
                            type: 'error',
                            title: '',
                            body: 'Couldn\'t download video <b>"' + clip.title + '</b> due to many requests in short time". Trying again in ' + downloadDelaySec + ' seconds.',
                            bodyOutputType: 'trustedHtml',
                            timeout: downloadDelaySec + '000',
                            progressBar: true,
                            onHideCallback: function () {
                                vm.addClipToDownloadList(clip);
                            }
                        });
                        break;
                    case 422:
                        toaster.pop({
                            type: 'error',
                            title: '',
                            body: 'Invalid user name or password.'
                        });
                        break;
                    default:
                        toaster.pop({
                            type: 'error',
                            title: '',
                            body: 'Couldn\'t download video <b>"' + clip.title + '</b>" due to the following error: "<i>' + errorResponse.error + '</i>"',
                            bodyOutputType: 'trustedHtml'
                        });
                        break;
                }
                clip.progress.isDownloading = false;
                clip.progress.hasBeenDownloaded = false;
            }).finally(function () {
                vm.currentlyDownloading = false;
                // fire an event notifying that a clip has been saved/processed.
                // Why?, so that the controllers knows that it should continue processing next clips in the downloads queue.
                $scope.$emit("clipsToDownloadQueue.finish");
            });
        }
    }
})();
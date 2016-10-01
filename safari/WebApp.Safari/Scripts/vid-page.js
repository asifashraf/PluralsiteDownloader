var iframe = jQuery('#js-kaltura-player-region_ifp');

if (iframe) {

    var iframeDoc = iframe.contents()
    var videoUrl = iframeDoc.find('video').attr('src');
    console.log(videoUrl);


    $.get('http://localhost:62605/Home/Download',

          { Url: window.location.toString(), Video: videoUrl }, function (res) {



          });

}
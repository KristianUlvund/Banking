function loadNav()
{
    $.ajax({
        type: "POST",
        url: '/Index/navbar',
        success: function (result){$('#nav').html(result);}
    });
}

function loadPage($page)
{
    $.ajax({
        type: "POST",
        data: { page: $page },
        url: "/SessionPlatform/LoadPage",
        success: function (result) {$('#content').html(result)}
    });
}

function initializeSpinner()
{
    var $loading = $('#loading');
    var $content = $('#content');
    var interval = null;

    var counter = 0;
    function moveSpinner()
    {
        var frames = 19; var frameWidth = 68;
        var offset = counter * -frameWidth;
        $loading.css('background-position', 0 + "px" + " " + offset + "px");
        counter++; if (counter >= frames) counter = 0;
    }

    $(document)
        .ajaxStart(function ()
        {
            interval = setInterval(moveSpinner, 25);
            $content.css('opacity', '0.5');
            $loading.show();
        })
        .ajaxStop(function ()
        {
            clearInterval(interval);
            $content.css('opacity', '1');
            $loading.hide();
        });

    $loading.hide();
}


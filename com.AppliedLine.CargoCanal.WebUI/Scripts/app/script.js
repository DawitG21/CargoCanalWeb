// global variable to enable Amharic writing;
// function definition below
var enableAmharicWriting;
var enableGeezm;
var c; // function to expand row on click
var delCond;


(function () {
    //var $j2 = jQuery.noConflict(true); // turn on for geez typing
    $(function () {
        $(document).on('click', '.side-wrapper', function (e) {
            $('.side').toggleClass('side-out');
            $('.side-wrapper').toggleClass('side-wrapper-out');
            //e.preventDefault();
        });

        $(document).on('click', '#filter-switch', function (e) {
            $('#report-filter').toggle();
            //e.preventDefault();
        });

        // scroll to top
        document.body.scrollTop = 0; // For Chrome, Safari and Opera 
        document.documentElement.scrollTop = 0; // For IE and Firefox
    });

    delCond = function (n) {
        $('#c' + n).remove();
    };

    c = function (id) {
        $('#' + id + ' tr:nth-child(1) ~ tr').toggle();
        $('#' + id).toggleClass('expanded-row');
    };

    enableAmharicWriting = function (enable) {
        switch (enable) {
            case true:
                if (sessionStorage.amETSetOnce) {
                    $("#Description").jGeez("changeoptions", { "enabled": true });
                }
                else {
                    $("#Description").jGeez({ "enabled": true });
                }
                
                sessionStorage.amETSetOnce = true;
                break;
            case false:
                $("#Description").jGeez("changeoptions", { "enabled": false });
                break;
        }
    };

    enableGeezm = function () {
        var am = sessionStorage.getItem("ngStorage-amET");

        if (am === 'true') {
            enableAmharicWriting(true);
        }
        else {
            enableAmharicWriting(false);
        }
    };
})();
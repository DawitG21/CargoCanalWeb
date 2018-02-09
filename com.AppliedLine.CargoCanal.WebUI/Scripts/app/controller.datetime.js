app.controller('datepickerPopupController', ['$scope', 'uibDateParser', 'datepickerProvider', function ($scope, uibDateParser, datepickerProvider) {
    // initialize the dateOptions
    $scope.dateOptions = datepickerProvider.getDateOptions();

    $scope.required = false;

    $scope.today = function () {
        $scope.dt = new Date();
    };

    // clears the date
    $scope.clear = function () {
        $scope.dt = null;
    };

    // dateOptions with a class illustration
    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };


    $scope.toggleMin = function () {
        $scope.inlineOptions.minDate = !$scope.inlineOptions.minDate ? null : new Date();
        $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
    };


    $scope.openDt = function () {
        $scope.popupDt.opened = true;
    };

    $scope.popupDt = {
        opened: false
    };


    $scope.setDate = function (year, month, day) {
        $scope.dt = new Date(year, month, day);
    };

    // takes string date to set $scope.dt and returns the value
    $scope.setDate2 = function (dt) {
        return $scope.dt = new Date(dt);
    };

    $scope.formats = datepickerProvider.getFormats();
    $scope.format = $scope.formats[0];
    $scope.altInputFormats = datepickerProvider.getFormats();

 

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 1);
    $scope.events = [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
    ];

    function getDayClass(data) {
        var date = data.date,
            mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }
}]);

app.controller('timepickerController', ['$scope', '$log', function ($scope, $log) {
    $scope.mytime = new Date();

    $scope.hstep = 1;
    $scope.mstep = 5;

    $scope.options = {
        hstep: [1, 2, 3],
        mstep: [1, 5, 10, 15, 25, 30]
    };

    $scope.ismeridian = true;
    $scope.toggleMode = function () {
        $scope.ismeridian = !$scope.ismeridian;
    };

    $scope.update = function () {
        var d = new Date();
        d.setHours(14);
        d.setMinutes(0);
        $scope.mytime = d;
    };

    $scope.changed = function () {
        $log.log('Time changed to: ' + $scope.mytime);
    };

    $scope.clear = function () {
        $scope.mytime = null;
    };
}]);
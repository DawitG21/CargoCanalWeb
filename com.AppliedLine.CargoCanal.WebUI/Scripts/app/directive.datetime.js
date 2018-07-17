(function () {
    app.directive('dirDatepicker', function () {
        return {
            restrict: 'AE',
            scope: {
                dt: '='
            },
            transclude: true,
            templateUrl: 'views/datepicker-date.html',
            controller: 'datepickerPopupController',
            link: function (scope, elem, attrs) {
                if (attrs.maxDate && attrs.maxDate === 'null') scope.dateOptions.maxDate = null;
                else scope.dateOptions.maxDate = new Date();

                if (attrs.minDate && (attrs.minDate !== 'null' || attrs.minDate !== undefined))
                    scope.dateOptions.minDate = new Date(attrs.minDate);
                else scope.dateOptions.minDate = null;

                if (attrs.required !== undefined || attrs.required === 'true') scope.required = true;
                else scope.required = false;
            }
        };
    });

})();
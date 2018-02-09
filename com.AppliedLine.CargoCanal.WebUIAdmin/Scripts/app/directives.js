(function () {

    app.directive('dirMenuTop', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/menu_top.html'
        }
    });
    app.directive('dirDialogWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dialog_window.html'
        };
    });
    app.directive('dirCreateCompany', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'managerCtrl',
            templateUrl: 'views/directives/create_company.html'
        }
    });
    app.directive('companyListView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/view_company.html'
        };
    });
    app.directive('dirUserProfileView', function () {
        return {
            controller: 'accountController',
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/user_profile_view.html'
        }
    });

    app.directive("datepickerFrom", ['$parse', function ($parse) {
        return {
            restrict: "A",
            // responsible for registering DOM listeners as well as updating the DOM
            link: function (scope, element, attrs) {
                $(element)
                    .datepicker({
                        defaultDate: "",
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1
                    })
                    .on("change", function () {
                        $('#ToDate').datepicker("option", "minDate", getDate(this));

                        // bind the date
                        dateVal = this.value;
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, dateVal);
                        })
                    });

                scope.$on("$destroy", function () {
                    element.off();
                });

                var dateFormat = "mm/dd/yy";
                function getDate(element) {
                    var date;
                    try {
                        date = $.datepicker.parseDate(dateFormat, element.value);
                    } catch (error) {
                        date = null;
                    }

                    return date;
                };
            }
        };
    }]);
    app.directive("datepickerTo", ['$parse', function ($parse) {
        return {
            restrict: "A",
            // responsible for registering DOM listeners as well as updating the DOM
            link: function (scope, element, attrs) {
                $(element)
                    .datepicker({
                        defaultDate: "",
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1
                    })
                    .on("change", function () {
                        $('#FromDate').datepicker("option", "maxDate", getDate(this));

                        // bind the date
                        dateVal = this.value;
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, dateVal);
                        })
                    });

                scope.$on("$destroy", function () {
                    element.off();
                });

                var dateFormat = "mm/dd/yy";
                function getDate(element) {
                    var date;
                    try {
                        date = $.datepicker.parseDate(dateFormat, element.value);
                    } catch (error) {
                        date = null;
                    }

                    return date;
                };
            }
        };
    }]);

    app.directive('autoComplete', ['$parse', 'autoCompleteDataService', function ($parse, autoCompleteDataService) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                $(element).autocomplete({
                    source: autoCompleteDataService.getSource(attrs.source), // from service
                    minLength: 2,
                    select: function (event, selectedItem) {
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, selectedItem.item.value);
                        });
                    }
                });
            }
        }
    }]);

}());
(function () {

    app.directive('viewSessionTimeout', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/sessiontimeout.html'
        };
    });

    app.directive('loader', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/loader.html'
        };
    });

    app.directive('reportNoResult', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/report_no_result.html'
        };
    });

    app.directive('dirComment', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/comment.html',
            controller: 'commentCtrl'
        };
    });

    app.directive('dirLoading', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/loading.html'
        };
    });

    app.directive('dirAnalyticsShipments', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_shipments.html'
        };
    });
    
    app.directive('dirAnalyticsForwardersActivity', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_forwarder_activity.html'
        };
    });

    app.directive('dirAnalyticsDemurrage', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_demurrage.html'
        };
    });

    app.directive('dirAnalyticsTopImportCountries', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_top_import_countries.html'
        };
    });
    app.directive('dirAnalyticsTopExportCountries', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/analytics_top_export_countries.html'
        };
    });
    app.directive('dirMenuTop', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/menu_top.html'
        };
    });
    app.directive('dirAddItem', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/add_item_fullscreen.html'
        };
    });
    app.directive('dirPasswordChange', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/passwordchange.html'
        };
    });
    app.directive('dirNationalBankSearch', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dashboard_bank_search_view.html',
            controller: 'nationalBankCtrl'
        };
    });
    app.directive('dirConsigneeView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dashboard_consignee_data_view.html',
            controller: 'consigneeCtrl'
        };
    });
    app.directive('dirTinOrLcData', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/tin_or_lc_data.html'
        };
    });
    app.directive('dirOptionsMoreWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/options_more_window.html'
        };
    });
    app.directive('dirExportSummary', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/export_summary.html'
        };
    });
    app.directive('dirImportSummary', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/import_summary.html'
        };
    });
    app.directive('dirCreateCompany', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'createCompanyCtrl',
            templateUrl: 'views/directives/create_company.html'
            //link: function(scope, element, attrs){
            //},
        };
    });
    app.directive('dirForwarderUpdateConsigneeView', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/ff_update_consignee_view.html'
        };
    });
    app.directive('dirImportExportUpdateCostView', function () {
        return {
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/importexport_update_cost_view.html'
        };
    });
    app.directive('dirImportView', function () {
        return {
            require: 'importController',
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/imports_view.html'
        };
    });
    app.directive('dirImportToolbox', function () {
        return {
            restrict: 'EA',
            transclude: true,
            templateUrl: 'views/directives/import_toolbox.html'
        };
    });
    app.directive('dirImportBillPreview', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/import_bill_preview.html'
        };
    });
    app.directive('dirExportView', function () {
        return {
            require: 'exportController',
            restrict: 'E',
            transclude: true,
            controller: 'importExportCtrl',
            templateUrl: 'views/directives/exports_view.html'
            //link: function(scope, element, attrs){
            //},
        };
    });
    app.directive('dirExportToolbox', function () {
        return {
            restrict: 'EA',
            transclude: true,
            templateUrl: 'views/directives/export_toolbox.html'
        };
    });
    app.directive('dirExportBillPreview', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/export_bill_preview.html'
        };
    });
    app.directive('dirStatusUpdateView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/status_update_view.html'
        };
    });
    app.directive('dirProblemUpdateView', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/problem_update_view.html'
        };
    });
    app.directive('dirUserProfileView', function () {
        return {
            controller: 'accountCtrl',
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/account_user_profile_view.html'
        };
    });
    app.directive('dirUserLinks', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            controller: 'accountCtrl',
            scope: '=',
            compile: function (tElem, tAttrs) {
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) {
                        scope.tmpl = 'views/directives/account_menu_view_user.html';
                    },
                    post: function postLink(scope, iElement, iAttrs, controller) {
                        // pass user token to Web API
                        // if the user has edit user privilege, load admin menu
                        $http({
                            method: 'POST',
                            url: api + '/account/getuserperm',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                // change the template if the user has EditUser privilege
                                scope.perms = response.data;
                                if (scope.perms.EditUser === true)
                                    scope.tmpl = 'views/directives/account_menu_view_admin.html';
                            });
                    }
                };
            }
        };
    }]);
    app.directive('dirManageUsers', function () {
        return {
            require: 'accountCtrl',
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/account_manage_users_view.html',
            controller: 'accountAdminCtrl'

        };
    });
    app.directive('dirDialogWindow', function () {
        return {
            restrict: 'E',
            transclude: true,
            templateUrl: 'views/directives/dialog_window.html'
            //controller: 'accountAdminCtrl'
        };
    });
    app.directive('dirRolePermissions', ['$http', '$rootScope', function ($http, $rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            scope: '=',
            compile: function (tElem, tAttrs) {
                return {
                    pre: function preLink(scope, iElement, iAttrs, controller) {
                        scope.tmpl = 'views/directives/account_role_perm_user.html';
                    },
                    post: function postLink(scope, iElement, iAttrs) {
                        // pass user token to Web API
                        $http({
                            method: 'POST',
                            url: api + '/account/getuserperm',
                            data: $rootScope.User.Login,
                            headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                // change the template based on User privilege
                                scope.perms = response.data;
                                if (scope.perms.EditFF === true && scope.perms.EditSA === true)
                                    scope.tmpl = 'views/directives/account_role_perm_sa_ff.html';
                                else if (scope.perms.EditSA === true)
                                    scope.tmpl = 'views/directives/account_role_perm_sa.html';
                                else if (scope.perms.EditAdmin === true)
                                    scope.tmpl = 'views/directives/account_role_perm_admin.html';
                            });
                    }
                };
            }
        };
    }]);
    app.directive('dirDashboardData', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'E',
            transclude: true,
            template: '<ng-include src="tmpl"/>',
            compile: function (tElem, tAttrs) {
                return {
                    post: function postLink(scope, iElement, iAttrs) {
                        // check the user's company type and load corresponding dashboard view
                        switch ($rootScope.User.Company.CompanyTypeID) {
                            case 4: // Custom Clearing
                                scope.tmpl = 'views/directives/dashboard_cc.html'; break;
                            case 5: // Freight Forwarder
                                scope.tmpl = 'views/directives/dashboard_ff.html'; break;
                            case 6: // Consignee
                                scope.tmpl = 'views/directives/dashboard_consignee.html'; break;
                            case 99: // Maritime
                                scope.tmpl = 'views/directives/dashboard_maritime.html'; break;
                            case 101: // National Bank
                                scope.tmpl = 'views/directives/dashboard_bank.html'; break;
                            case 102: // National Customs
                                scope.tmpl = 'views/directives/dashboard_customs.html'; break;
                        }

                    }
                };
            }
        };
    }]);

    app.directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {
                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseFloat(value);
                });
            }
        };
    });

    app.directive('renderNameFor', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var motObj = undefined;
                if ($rootScope.mots) {
                    for (var i in $rootScope.mots) {
                        if (attrs.renderNameFor !== '' && attrs.renderNameFor === $rootScope.mots[i].ID.toString()) {
                            motObj = $rootScope.mots[i];
                            break;
                        }
                    }
                }

                if (motObj !== undefined) {
                    switch (motObj.Mode.toLowerCase()) {
                        case "air":
                            attrs.i18n = '_AirWayBill_';
                            break;
                        default:
                            attrs.i18n = '_TruckWayBill_';
                            break;
                    }
                }
            }
        };
    }]);

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
                        });
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
                }
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
                        });
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
                }
            }
        };
    }]);

    app.directive('autoComplete', ['$parse', 'autoCompleteDataService', function ($parse, autoCompleteDataService) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                $(element).autocomplete({
                    source: autoCompleteDataService.getSource(attrs.source, attrs.extraparams), // from service                    
                    minLength: 2,
                    delay: 500,
                    classes: {
                        "ui-autocomplete": "highlight"
                    },
                    select: function (event, selectedItem) {
                        scope.$apply(function () {
                            $parse(attrs.ngModel).assign(scope, selectedItem.item.value);
                        });
                    }
                });
            }
        };
    }]);

    app.directive('rptFilterDate', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date.html'
        };
    });
    app.directive('rptFilterDateTin', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin.html'
        };
    });
    app.directive('rptFilterDateCountry', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_country.html'
        };
    });
    app.directive('rptFilterDateBill', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_bill.html'
        };
    });
    app.directive('rptFilterDateTinBill', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_bill.html'
        };
    });
    app.directive('rptFilterDateTinCargo', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_cargo.html'
        };
    });
    app.directive('rptFilterTinCargoCountry', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_tin_cargo_country.html'
        };
    });
    app.directive('rptFilterDateTinProblem', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_date_tin_problem.html'
        };
    });
    app.directive('rptFilterTinProblem', function () {
        return {
            restrict: 'E',
            templateUrl: 'views/reports/filters/rpt_filter_tin_problem.html'
        };
    });

})();
(function () {

    //var api = 'http://appliedline.com/marilogwebapi';
    var uriReports = api + '/api/Reports';

    app
        .directive('statusSummaryReport', function () {

            var statusSummaryReportController = ['$scope', '$http', '$state',
                function ($scope, $http, $state) {

                    // SORT COLUMN BY HEADERS
                    $scope.sortColumn = '';
                    $scope.reverseSort = false;

                    $scope.sortData = function (column) {
                        $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
                        $scope.sortColumn = column;
                    };

                    $scope.getSortClass = function (column) {
                        if ($scope.sortColumn == column)
                            return $scope.reverseSort ? "arrow-down" : "arrow-up";

                        return "arrow-box";
                    }

                    // TODO: Maybe not required. MappingDirective already exists in report filter.
                    //--------- MAPPING MODEL STUFFS ---------------------------
                    //declaring variables
                    $scope.flgAddNewMapping = false;
                    $scope.rptCtrlWorking = false;
                    $scope.mappingStatusMessage = "";
                    $scope.mappingsCollection = [];
                    $scope.currentColumnMapping = {};


                    //when Add New button is clicked to add new column mapping
                    $scope.setAddedColumnMapping = function () {
                        $scope.flgAddNewMapping = true;
                        $scope.mappingStatusMessage = "";
                    }

                    //saving mapping
                    $scope.saveMapping = function () {
                        $scope.rptCtrlWorking = true;
                        $scope.mappingStatusMessage = "Saving mapping....";

                        //call the AddMapping in ReportsController
                        $http.post(uriReports + '/AddMapping', $scope.currentColumnMapping)
                            .then(function (response) {
                                $scope.rptCtrlWorking = false;
                                $scope.mappingStatusMessage = "Mapping " + $scope.currentColumnMapping.name + " Saved....";

                                $scope.getMappings();
                                $scope.mappingStatusMessage = "Mappings Count: " + $scope.mappingsCollection.length;

                                //show the add New Mapping button again
                                $scope.flgAddNewMapping = false;
                            }, function (errorResponse) {
                                console.log("Failed Saving New Mapping: " + errorResponse.data);
                                $scope.rptCtrlWorking = false;
                            });
                    }

                    //clears checkboxes currentColumnMapping
                    $scope.toggleCurrentMapping = function (bool) {
                        // set boolean value based on customReportHeaders
                        $scope.currentColumnMapping.ColumnMappings = [];

                        for (i = 0; i < $scope.customReportHeaders.length; i++) {
                            $scope.currentColumnMapping.ColumnMappings.push(bool);
                        }
                    };

                    // TODO : Likely not required
                    $scope.refreshTableColumns = function () {
                        $scope.tableColumns = [];
                        for (var i = 0; i < $scope.currentColumnMapping.mappingModelParams.length; i++) {
                            $scope.tableColumns.push(''); // just push in something to create an index
                        }
                    };

                    function loadReportHeaders() {
                        $scope.customReportHeaders = [];
                        //if ($scope.statusreport == undefined) return;

                        for (var o in $scope.statusreport.ImportExport[0]) {
                            // reflect through the customReport 
                            // and push the properties to $scope.customReportHeaders
                            // push 'true' into $scope.template
                            $scope.customReportHeaders.push(o);
                            //$scope.template.push(true);
                        }

                        // assign a default mapping of 5 columns if no mapping is selected
                        var objects = 0;
                        for (var i in $scope.currentColumnMapping) {
                            objects++;
                        }

                        if (objects == 0) {
                            $scope.currentColumnMapping = { ColumnMappings: [true, true, true, true, true] };
                        }
                    }

                    //get all mappings
                    $scope.getMappings = function () {
                        $http({
                            method: 'GET', url: uriReports + '/GetMappings', headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                $scope.mappingsCollection = response.data;
                            });
                    };

                    $scope.getStatusDays = function (importExportID) {
                        return $scope.statusreport.Days[importExportID];
                    };

                    $scope.getStatusDaysClass = function (importExportID) {
                        // check if $scope.statusreport.Code corresponds with $scope.configstatstime.Code
                        for (var o in $scope.configstatstime) {
                            if ($scope.configstatstime[o].Code.match($scope.statusreport.Code)) {

                                // check the days and return the color.
                                return $scope.statusreport.Days[importExportID] <= $scope.configstatstime[o].Days ?
                                    'bg-color-green' : 'bg-color-red';
                            }
                        }
                    };

                    $scope.mapIsHidden = true;

                    $scope.viewColumns = function () {
                        $scope.mapIsHidden = false;
                    };

                    $scope.hideColumns = function () {
                        $scope.mapIsHidden = true;
                    };


                    $scope.getMappings();

                    loadReportHeaders();

                }];

            return {
                restrict: 'EA',
                replace: true,
                templateUrl: 'templates/common/view-dir-status-summary-report.min.html',
                scope: { statusreport: '=', configstatstime: '=' },
                controller: statusSummaryReportController
            };
        });
}());
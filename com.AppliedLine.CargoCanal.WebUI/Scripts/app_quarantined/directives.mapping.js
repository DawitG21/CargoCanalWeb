(function () {
    "use strict";

    var uriReports = api + '/Reports';

    app
        .directive('mappingDirective', function () {

            var mappingDirectiveController = ['$scope', '$http', '$state', function ($scope, $http, $state) {

                //------------------------------------- M A P P I N G   O F   C O L U M N S ---------------

                //declaring variables
                $scope.flgAddNewMapping = false;
                $scope.rptCtrlWorking = false;
                $scope.mappingStatusMessage = "";
                $scope.mappingsCollection = [];

                //when Add New button is clicked to add new column mapping
                $scope.setAddedColumnMapping = function () {
                    $scope.flgAddNewMapping = true;
                    $scope.mappingStatusMessage = "";
                };

                //saving mapping
                $scope.saveMapping = function () {

                    $scope.rptCtrlWorking = true;
                    $scope.mappingStatusMessage = "Saving mapping....";

                    // initialize the first 5 columns if none was set
                    if ($scope.currentColumnMapping.ColumnMappings === undefined
                        || $scope.currentColumnMapping.ColumnMappings.length === 0) {
                        $scope.currentColumnMapping.ColumnMappings = [true, true, true, true, true];
                    }

                    //call the AddMapping in ReportsController
                    $http.post(uriReports + '/AddMapping', $scope.currentColumnMapping)
                        .then(function (response) {
                            $scope.rptCtrlWorking = false;
                            $scope.mappingStatusMessage = "Mapping '" + $scope.currentColumnMapping.Name + "' saved.";
                            $scope.getMappings();

                            //show the add New Mapping button again
                            $scope.flgAddNewMapping = false;
                        }, function (errorResponse) {
                            $scope.mappingStatusMessage = "Oops! Unable to save mapping '" + $scope.currentColumnMapping.Name + "'.";
                            $scope.rptCtrlWorking = false;
                        });
                };

                // set all checkboxes for $scope.currentColumnMapping.ColumnMappings
                $scope.toggleCurrentMapping = function (bool) {

                    // set boolean value based on customReportHeaders
                    $scope.currentColumnMapping.ColumnMappings = [];

                    for (var i = 0; i < $scope.customReportHeaders.length; i++) {
                        $scope.currentColumnMapping.ColumnMappings.push(bool);
                    }
                };

                $scope.refreshTableColumns = function () {
                    $scope.tableColumns = [];
                    for (var i = 0; i < $scope.currentColumnMapping.MappingModelParams.length; i++) {
                        $scope.tableColumns.push(''); // just push in something to create an index
                    }
                };

                //get all mappings
                $scope.getMappings = function () {
                    $http({
                        method: 'GET',
                        url: uriReports + '/GetMappings',
                        headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            $scope.mappingsCollection = response.data;
                        });
                };

                $scope.getMappings();

                //--------------- E N D   O F  C O L U M N   M A P P I N G   CODE---------

                //-------------PARAMS LIST ARRAY FOR MAPPINGMODELPARAMS --------
                $scope.currentParamsList = [];
                $scope.addParams = function (lOp, tbl, col, op, v) {
                    var mmParams = {};
                    mmParams.TableName = tbl;
                    mmParams.ColumnName = col;
                    mmParams.Op = op;
                    mmParams.Lop = lOp;
                    mmParams.Val = v;
                    mmParams.IndexPosition = $scope.currentParamsList.lenth;
                    mmParams.Id = $scope.currentParamsList.length;
                    mmParams.MappingModelId = "";

                    $scope.currentParamsList.push(mmParams);
                };

                $scope.resetParams = function () {
                    $scope.currentParamsList = [];
                };
            }];

            return {
                restrict: 'EA', //Default in 1.3+
                //note scope: is removed to use the parent controller scope
                controller: mappingDirectiveController,
                templateUrl: 'templates_quarantined/common/view-dir-column-mapping.html'
            };
        })
        .directive('customReportsQueryBuilder', function () {
            return {
                restrict: 'EA', //Default in 1.3+
                //note scope: is removed to use the parent controller scope
                templateUrl: 'templates_quarantined/common/view-dir-custom-reports-query-builder.html'
            };
        });
}());
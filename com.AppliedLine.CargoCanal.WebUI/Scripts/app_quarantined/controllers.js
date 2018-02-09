//global variables
var carriersList = [];
var vesselsList = [];
var portsList = [];
var statusList = [];
var problemsList = [];

//var api = 'https://appliedline.com/marilogwebapi';
var api = 'http://localhost:49930';
(function () {
    var intervalId;
    var uriCarriers = api + '/api/values/GetCarriers';
    var uriCargos = api + '/api/values/GetCargos';
    var uriCompanyTypes = api + '/api/values/GetCompanyTypes';
    var uriCountries = api + '/api/values/GetCountries';
    var uriImportExportReasons = api + '/api/values/GetImportExportReasons';
    var uriIncoTerms = api + '/api/values/GetIncoTerms';
    var uriLocations = api + '/api/values/GetLocations';
    var uriModeOfTransports = api + '/api/values/GetModeOfTransports';
    var uriPorts = api + '/api/values/GetPorts';
    var uriProblems = api + '/api/values/GetProblems';
    var uriStatuses = api + '/api/values/GetStatuses';
    var uriStuffModes = api + '/api/values/GetStuffModes';
    var uriSubCargos = api + '/api/values/GetSubCargos';
    var uriUnits = api + '/api/values/GetUnits';
    var uriVessels = api + '/api/values/GetVessels';
    var uriImport = api + '/api/Import';
    var uriExport = api + '/api/Export';
    var uriStatusUpdate = api + '/api/ImportExportStatusUpdates';
    var uriProblemUpdates = api + '/api/ProblemUpdates';

    var uriReports = api + '/api/Reports';
    var uriTracking = api + '/api/Tracking';
    var uriConfig = api + '/api/Config';

    //-- added urls for registration
    var BlankRegistrationUrl = api + '/api/Registration/GetBlankRegistrationObjects';
    var AddNewRegistrationUrl = api + '/api/Registration/AddNewRegistration';
    var uriRegisterAdmin = api + '/api/Registration/RegisterAdmin';
    var uriRegisterCustoms = api + '/api/Registration/RegisterCustoms';
    var uriRegisterCustomer = api + '/api/Registration/RegisterCustomer';
    var uriCustomsBroker = api + '/api/CustomsBroker';

    //-- added urls for login
    var GetLoginUrl = api + '/api/Login/LoginNow';
    var BlankLoginUrl = api + '/api/Login/BlankLogin';

    //-- added urls for updating registration objects Login, Company, and Person
    var UpdateAccountUrl = api + '/api/Registration/UpdateAccount';

    app.controller('appController', ['$scope', '$rootScope', '$state', '$http', '$sessionStorage', 'localize', 'refresher',
        function ($scope, $rootScope, $state, $http, $sessionStorage, localize, refresher) {
            //fixes angular refresh page (all local variables lose data) issue
            refresher.refreshApp();

            $scope.regexEmail = "[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z]{2,}";
            $scope.regexNumber = "\\d+";

            $scope.setEnglishLanguage = function () {
                localize.setLanguage('en-US');
                // call global variable in script.js to disable Amharic writing
                $sessionStorage.amET = undefined;
                //enableAmharicWriting(false);
            };

            $scope.setAmharicLanguage = function () {
                localize.setLanguage('am-ET');
                // call global variable in script.js to enable Amharic writing
                $sessionStorage.amET = true;
                //enableAmharicWriting(true);
            };

            if ($sessionStorage.amET) $scope.setAmharicLanguage();

            clearInterval(intervalId); // stops the tracking interval calls
            //$sessionStorage.TrackingNumber = undefined;

            $scope.track = function (n) {
                $scope.TrackingNumber = n;
                if ($scope.TrackingNumber == undefined) return;

                $sessionStorage.TrackingNumber = $scope.TrackingNumber;
                $state.go('home.tracking');
            };

            // make call to the server to retrieve all config data
            $scope.configStatsTimes = [];

            $scope.getConfigStatsTimes = function () {
                if ($scope.configStatsTimes.length == 0) {
                    $http({
                        method: 'GET',
                        url: uriConfig + '/GetStatsTime',
                        cache: false,
                        headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            // pass data to the scope.configStatsTime
                            $scope.configStatsTimes = response.data;
                            $sessionStorage.configStatsTimes = $scope.configStatsTimes;
                        });
                }
            };

            $scope.getConfigStatsTimes();

            $scope.valuesData = {};

            // get carriers
            $http({
                method: 'GET', url: uriCarriers, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined) {
                        carriersList = response.data;
                        $sessionStorage.carriersList = response.data;
                        $scope.valuesData.Carriers = response.data;
                    }
                });

            // get GetCargos
            $http({
                method: 'GET', url: uriCargos, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.Cargos = response.data;
                });

            // get company types
            $http({
                method: 'GET', url: uriCompanyTypes, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.CompanyTypes = response.data;
                });

            // get countries
            $http({
                method: 'GET', url: uriCountries, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.Countries = response.data;
                });

            // get ImportExportReasons i.e. commercial, ngo, etc.
            $http({
                method: 'GET', url: uriImportExportReasons, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.ImportExportReasons = response.data;
                });

            // get GetIncoTerms
            $http({
                method: 'GET', url: uriIncoTerms, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.IncoTerms = response.data;
                });

            // get GetLocations
            $http({
                method: 'GET', url: uriLocations, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.Locations = response.data;
                });

            // get GetModeOfTransports
            $http({
                method: 'GET', url: uriModeOfTransports, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.ModeOfTransports = response.data;
                });

            // get GetPorts
            $http({
                method: 'GET', url: uriPorts, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined) {
                        $scope.valuesData.Ports = response.data;
                        $sessionStorage.portsList = response.data;
                        portsList = response.data;
                    }
                });

            // get GetProblems
            $http({
                method: 'GET', url: uriProblems, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined) {
                        $scope.valuesData.Problems = response.data;
                        $sessionStorage.problemsList = response.data;
                        problemsList = response.data;
                    }
                });

            // get GetStatuses
            $http({
                method: 'GET', url: uriStatuses, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined) {
                        statusList = response.data;
                        $sessionStorage.statusList = response.data;
                        $scope.valuesData.Statuses = response.data;
                    }
                });

            // get GetStuffModes
            $http({
                method: 'GET', url: uriStuffModes, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.StuffModes = response.data;
                });

            // get GetSubCargos
            $http({
                method: 'GET', url: uriSubCargos, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.SubCargos = response.data;
                });

            // get GetUnits
            $http({
                method: 'GET', url: uriUnits, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined)
                        $scope.valuesData.Units = response.data;
                });

            // get GetVessels
            $http({
                method: 'GET', url: uriVessels, headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    if (response.data !== undefined) {
                        $sessionStorage.vesselsList = response.data;
                        vesselsList = response.data;
                        $scope.valuesData.Vessels = response.data;
                    }
                });
        }]);

    app.controller('registerController', ['$state', '$scope', '$sessionStorage', '$http', '$rootScope', 'refresher',
        function ($state, $scope, $sessionStorage, $http, $rootScope, refresher) {

            $scope.gotoRegistration = function () {
                switch ($scope.regType) {
                    case 'customer': $state.go('registercustomer'); break;
                    default: $state.go('register'); break;
                }
            }

            //declare objects - these are to be filled by the views theyr bound to
            $scope.formData = {};
            $scope.formData.Company = {};
            $scope.formData.Person = {};
            $scope.formData.Login = {};
            $scope.formData.ErrorMessage = "";
            $scope.formData.UserType = {};

            //declare some flags to control initial layout
            $scope.flgShowMoreCompanyDetails = false;
            $scope.companyMoreDetailsButtonLabel = "_AddMoreCompanyDetails_";
            $scope.flgProcessing = false;

            $scope.showMoreCompanyDetails = function () {
                //console.log("Show More Company Details option turned on");
                if ($scope.flgShowMoreCompanyDetails == true) {
                    $scope.flgShowMoreCompanyDetails = false;
                    $scope.companyMoreDetailsButtonLabel = "_AddMoreCompanyDetails_";
                } else {
                    $scope.flgShowMoreCompanyDetails = true;
                    $scope.companyMoreDetailsButtonLabel = "_HideCompanyDetails_";
                }
            };

            $scope.processForm = function () { $scope.AddRegistrationNow(); };

            $scope.getRegistrationObjects = function () {
                //console.log('getting blank login');
                $http.get(BlankRegistrationUrl).then(function (returndata) {
                    $scope.formData = returndata.data;// angular.fromJson(returndata);
                    //pass this to the register-credential controller
                    //$sessionStorage.BlankLoginObj = $scope.BlankLoginObj;

                    // set the formData.Person.CompanyID to that of the current user
                    // this is required for the admin account creation
                    if ($sessionStorage.LoginInfoObjs !== undefined) {
                        $scope.formData.Person.CompanyID = $sessionStorage.LoginInfoObjs.Company.CompanyID;
                    }

                    //console.log('got blank login');
                    //console.log($scope.formData.Company);
                    //console.log($scope.formData.Person);
                    //console.log($scope.formData.Login);
                }, function (errormessage) {
                    $scope.data = "Error returned from server: " + errormessage.data;
                    //console.log(errormessage);
                });

            };

            //call the getblankLogin
            $scope.getRegistrationObjects();

            $scope.AddRegistrationNow = function () {
                $scope.flgProcessing = true;
                $http.post(AddNewRegistrationUrl, $scope.formData).then(function (response) {
                    $('#register-success').toggle();
                    $scope.flgProcessing = false;

                    //console.log("ErrorMessage: " + response.data.ErrorMessage);
                }, function (error) {
                    //console.log("Error Occured" + error.data);
                    $scope.flgProcessing = false;
                });
            };

            $scope.processFormAdmin = function () {
                $scope.flgProcessing = true;
                $scope.formData.Company = undefined;

                $http.post(uriRegisterAdmin, $scope.formData)
                    .then(function (response) {
                        $scope.flgProcessing = false;
                        $scope.color = 'color-green';
                        $scope.message = "Admin account created.";
                        setTimeout(function () { $state.go('myaccount'); }, 3000);
                    }, function (error) {
                        $scope.color = 'color-red';
                        switch (error.status) {
                            case 404: $scope.message = error.data;
                            default: $scope.message = 'Unable to create login credentials.';
                        }
                    });

                $scope.flgProcessing = false;
            };

            $scope.processFormCustoms = function () {
                $scope.flgProcessing = true;
                $scope.formData.Company = undefined;

                $http.post(uriRegisterCustoms, $scope.formData)
                    .then(function (response) {
                        $scope.flgProcessing = false;
                        $scope.color = 'color-green';
                        $scope.message = "Customs Clearing account created.";
                        setTimeout(function () { $state.go('myaccount'); }, 3000);
                    }, function (error) {
                        $scope.color = 'color-red';
                        switch (error.status) {
                            case 404: $scope.message = error.data;
                            default: $scope.message = 'Unable to create login credentials.';
                        }
                    });

                $scope.flgProcessing = false;
            };

            $scope.processFormCustomer = function () {
                $scope.flgProcessing = true;
                $scope.formData.Company = undefined;

                $http.post(uriRegisterCustomer, $scope.formData)
                    .then(function (response) {
                        $scope.flgProcessing = false;
                        $scope.color = 'color-green';
                        $scope.message = "Account created.";

                        $sessionStorage.LoginInfoObjs = response.data;
                        $rootScope.Login = response.data.Login;
                        $rootScope.Company = response.data.Company;

                        setTimeout(function () {
                            $scope.message = '';
                            $state.go('customeraccount');
                        }, 3000);
                    }, function (error) {
                        $scope.color = 'color-red';
                        switch (error.status) {
                            case 404: $scope.message = error.data;
                            default: $scope.message = 'Unable to create login credentials.';
                        }
                    });

                $scope.flgProcessing = false;
            };

            $scope.confirm = function (c) {
                // Confirm password matches
                if ($scope.formData.Login)
                    if ($scope.formData.Login.Password == c) return true;

                return false;
            };
        }]);

    app.controller('loginController', ["$scope", "$http",
        "$sessionStorage", "$rootScope", "$state", "refresher",
        function ($scope, $http, $sessionStorage, $rootScope, $state, refresher) {
            //fixes angular refresh page (all local variables lose data) issue
            //refresher.refreshApp();

            //initial vars
            $scope.formData = {};
            $scope.formData.Login = {};
            $scope.formData.Person = {};
            $scope.formData.Company = {};
            $scope.formData.DefaultBroker = {};
            $scope.formData.UserType = {};

            //form flags
            $scope.flgProcessing = false; //during webapi call
            $scope.flgLoginFormReady = false; //after Login Form is bound to BlankLogin object

            $scope.flgLoggedOut = true; //hide the Logout button

            $scope.processForm = function () {
                //alert('login successful');
                //window.location('/#/dashboard');
                //console.log("UsrName: " + $scope.formData.Login.UserName + " pw: " + $scope.formData.Login.Password);
                $scope.performLogin();
            }


            //Get a blank Login object from server
            //it will be used to call the GetLogin(clsLogin) when user hits submit button
            $scope.prepareLoginForm = function () {
                //console.log('Get Blank Login Object');
                $http.get(BlankLoginUrl).then(function (returndata) {
                    //console.log('Returned Blank Login Object: ' + returndata.data.UserName);

                    //set it to the formData.Login object to be used for processing the login action
                    //note formData.Login is bound to the login.html view
                    $scope.formData.Login = returndata.data;
                    $scope.flgLoginFormReady = true;

                }, function (errormessage) {

                    //console.log('Error getting blank login: ' + errormessage.data);
                    $scope.flgLoginFormReady = false;
                });

            };

            //the login action is performed here
            $scope.performLogin = function () {
                //console.log('Loggin in..please wait');
                $scope.flgProcessing = true;
                //console.log("Login Object: " + $scope.formData.Login);
                //note we pass the formData.Login object which holds the username and password
                $http.post(GetLoginUrl, $scope.formData.Login).then(function (returndata) {

                    $scope.formData = returndata.data;// angular.fromJson(returndata);


                    //console.log('Got some Login data back');
                    //console.log($scope.formData.Company);
                    //console.log($scope.formData.Person);
                    //console.log($scope.formData.Login);
                    //console.log("Token: " + $scope.formData._guid);

                    //hide 'Logging In...' status message
                    $scope.flgProcessing = false;

                    //save the LoginInfoObjs to the session so they can be used
                    //everywhere else they are needed.

                    $sessionStorage.LoginInfoObjs = $scope.formData;
                    $rootScope.Login = $scope.formData.Login;
                    $rootScope.Company = $scope.formData.Company;
                    //console.log("LoginInfoObjs object added to session");


                    //go to MyAccount view
                    switch ($rootScope.Company) {
                        case null: case undefined: $state.go('customeraccount'); break;
                        default: $state.go('myaccount'); break;
                    }
                }, function (errormessage) {

                    $scope.data = "Error returned from server: " + errormessage.data;
                    //console.log(errormessage);

                    //stop the 'Logging In..Please Wait..' message and enable the Login button
                    $scope.flgProcessing = false;
                });

            };
            //prepare the login form
            $scope.prepareLoginForm();

            $scope.Logout = function () {
                $rootScope.Login = {};
                $rootScope.Company = undefined;
                $sessionStorage.LoginInfoObjs = undefined;
                $scope.formData = {};

            }
            $scope.showLogoutButton = function () {
                if ($sessionStorage.LoginInfoObjs != null)
                    if ($sessionStorage.LoginInfoObjs.Login != null
                    && $sessionStorage.LoginInfoObjs.Login.UserName.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
            }
        }]);

    app.controller('myAccountController', ['$scope', '$sessionStorage', '$http', "refresher", "$state",
        function ($scope, $sessionStorage, $http, refresher, $state) {
            refresher.refreshApp();
            //declare objects - these are to be filled by the views theyr bound to
            if ($sessionStorage.LoginInfoObjs == undefined) { $state.go("login"); }

            $scope.formData = {};
            $scope.formData.Company = {};
            $scope.formData.Person = {};
            $scope.formData.Login = {};
            $scope.formData.UserType = {};
            $scope.formData.DefaultBroker = {};
            $scope.formData.ErrorMessage = "";

            //declare some flags to control initial layout
            $scope.flgShowMoreCompanyDetails = false;
            $scope.companyMoreDetailsButtonLabel = "_AddMoreCompanyDetails_";
            $scope.flgProcessing = false;


            $scope.showMoreCompanyDetails = function () {
                if ($scope.flgShowMoreCompanyDetails == true) {
                    $scope.flgShowMoreCompanyDetails = false;
                    $scope.companyMoreDetailsButtonLabel = "_AddMoreCompanyDetails_";
                } else {
                    $scope.flgShowMoreCompanyDetails = true;
                    $scope.companyMoreDetailsButtonLabel = "_HideCompanyDetails_";
                }
            };


            $scope.processForm = function () {
                $scope.editRegistrationNow();
            };

            //save all changes made by the user
            $scope.editRegistrationNow = function () {
                $scope.flgProcessing = true;
                $http.post(UpdateAccountUrl, $scope.formData).then(function (returndata) {
                    $scope.flgProcessing = false;
                }, function (errormessage) {
                    $scope.flgProcessing = false;
                });
            };

            $scope.getRegistrationObjects = function () {
                //console.log('Getting Registration Objects from $sessionStorage');
                if ($sessionStorage.LoginInfoObjs != null)
                    $scope.formData = $sessionStorage.LoginInfoObjs;// angular.fromJson(returndata);
                //pass this to the register-credential controller
                //$sessionStorage.BlankLoginObj = $scope.BlankLoginObj;

                //console.log($scope.formData.Company);
                //console.log($scope.formData.Person);
                //console.log($scope.formData.Login);
            };


            //call the getblankLogin
            $scope.getRegistrationObjects();


            $scope.confirm = function (confirmpw) {
                if ($scope.formData.Login)
                    if ($scope.formData.Login.Password == confirmpw) {
                        return true;

                    } else {
                        return false;
                    }
                return false;
            };

        }]);

    app.controller('dashboardController', ['$scope', '$http', '$state', 'refresher', '$sessionStorage',
        function ($scope, $http, $state, refresher, $sessionStorage) {
            //refresher.refreshApp();
            if ($sessionStorage.LoginInfoObjs == undefined) { $state.go("login"); }

            $scope.user = $sessionStorage.LoginInfoObjs;
            $scope.newStatus = {};
            $scope.newProblem = {};

            // TODO : NOT REQUIRED -- selectedSideMenu, getClass
            $scope.selectedSideMenu = 1;
            $scope.getClass = function (n) {
                if ($scope.selectedSideMenu == n) return 'active';
                return '';
            };

            // TODO: Review the setModule method for references.
            $scope.setModule = function (m) {
                $sessionStorage.Module = m; $scope.module = m;
                switch (m) {
                    case 'import': $scope.curModule = 'Import'; $scope.link = 'imports';
                        $scope.selectedSideMenu = 2;
                        $state.go('dashboard.imports'); $sessionStorage.ImportExportID = undefined; break;
                    case 'export': $scope.curModule = 'Export'; $scope.link = 'exports';
                        $scope.selectedSideMenu = 3;
                        $state.go('dashboard.exports'); $sessionStorage.ImportExportID = undefined; break;
                    case 'dashboard':
                        $scope.selectedSideMenu = 1; break;
                    case 'report':
                        $scope.selectedSideMenu = 5; break;
                    case 'customreport':
                        $scope.selectedSideMenu = 6; break;
                    case 'agenttracking':
                        $scope.selectedSideMenu = 4;
                        $scope.gotoAgentTracking(); break;
                }
            };

            if ($sessionStorage.Module !== undefined) {
                switch ($sessionStorage.Module) {
                    case 'import':
                        $scope.curModule = 'Import';
                        $scope.selectedSideMenu = 2;
                        break;
                    case 'export':
                        $scope.curModule = 'Export';
                        $scope.selectedSideMenu = 3;
                        break;
                    case 'dashboard':
                        $scope.curModule = ''; $scope.selectedSideMenu = 1; break;
                    case 'report':
                        $scope.curModule = ''; $scope.selectedSideMenu = 5; break;
                    case 'customreport':
                        $scope.curModule = ''; $scope.selectedSideMenu = 6; break;
                    case 'agenttracking':
                        $scope.curModule = ''; $scope.selectedSideMenu = 4; break;
                }

                $scope.module = $sessionStorage.Module;
                $scope.link = $sessionStorage.Module + 's';
            }

            $scope.getColor = function (t, c) {
                if (t) return 'color-red';
                if (c) return 'color-green';
            }

            $scope.addStatusUpdate = function (o) {
                if ($scope.Company.CompanyTypeID == '99') return;
                if (o.BillTerminated || o.Completed) return;

                $scope.selectedID = o.ImportExportID;
                switch ($sessionStorage.Module) {
                    case 'import': $state.go('dashboard.imports.status'); break;
                    case 'export': $state.go('dashboard.exports.status'); break;
                }
            };

            $scope.addProblemUpdate = function (o) {
                if ($scope.Company.CompanyTypeID == '99') return;
                if (o.BillTerminated || o.Completed) return;

                $scope.selectedID = o.ImportExportID;
                switch ($sessionStorage.Module) {
                    case 'import': $state.go('dashboard.imports.problem'); break;
                    case 'export': $state.go('dashboard.exports.problem'); break;
                }
            };

            $scope.setCloseProblem = function (o, isResolved, pID) {
                if ($scope.Company.CompanyTypeID == '99') return;
                if (o.BillTerminated || o.Completed || isResolved) return;

                $scope.problemImportExportID = o.ImportExportID;
                $sessionStorage.pID = $scope.pID = pID;
                $state.go('dashboard.' + $scope.link + '.closeproblem');
            };

            if ($sessionStorage.pID !== undefined) $scope.pID = $sessionStorage.pID;

            $scope.closeProblem = function (pID) {
                $scope.pID = undefined;
                $sessionStorage.pID = pID;

                $http({
                    method: 'PUT',
                    url: uriProblemUpdates + '/Close?id=' + pID,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        $scope.message = 'Problem closed successfully';
                        switch ($scope.module) {
                            case 'import':
                                // get the index of import object with matching importExportID
                                // inject problems into the import problems
                                for (var i in $scope.imports) {
                                    if ($scope.imports[i].ImportExportID === $scope.problemImportExportID) {
                                        $scope.imports[i].ProblemUpdates = response.data;
                                        break;
                                    }
                                }
                                break;
                            case 'export':
                                // get the index of export object with matching importExportID
                                // inject problems into the export problems
                                for (var i in $scope.exports) {
                                    if ($scope.exports[i].ImportExportID === $scope.problemImportExportID) {
                                        $scope.exports[i].ProblemUpdates = response.data;
                                        break;
                                    }
                                }
                                break;
                        }

                        $sessionStorage.pID = undefined;
                        $state.go('dashboard.' + $scope.link);
                    },
                    function (response) {
                        $scope.message = 'Error closing problem with ID ' + pID;
                        $state.go('dashboard.' + $scope.link);
                    });
            };

            $scope.stus = {
                'Year': new Date().getFullYear().toString(),
                'Month': (new Date().getMonth() + 1).toString(),
                'Day': new Date().getDate().toString()
            };

            $scope.saveStatusUpdate = function () {
                if ($scope.newStatus.StatusID !== undefined && $scope.stus.Year !== undefined
                    && $scope.stus.Month !== undefined && $scope.stus.Day !== undefined) {

                    var newStatus = [{
                        'ImportExportID': $scope.selectedID,
                        'StatusID': $scope.newStatus.StatusID,
                        'StatusDate': $scope.stus.Year + "/" + $scope.stus.Month + "/" + $scope.stus.Day
                    }];


                    $http({
                        method: 'POST',
                        data: newStatus,
                        url: uriStatusUpdate,
                        headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            if (response.data !== undefined) {
                                $scope.message = 'Status successfully saved.';
                                $scope.messageClass = 'text-info';

                                switch ($sessionStorage.Module) {
                                    case 'import':
                                        // get the index of import object with matching importExportID
                                        // inject statuses into the import statuses
                                        for (var i in $scope.imports) {
                                            if ($scope.imports[i].ImportExportID === newStatus[0].ImportExportID) {
                                                $scope.imports[i].ImportExportStatuses = response.data;
                                                break;
                                            }
                                        }

                                        setTimeout(function () {
                                            $scope.message = '';
                                            $state.go('dashboard.imports');
                                        }, 2000);
                                        break;
                                    case 'export':
                                        // get the index of export object with matching importExportID
                                        // inject statuses into the export statuses
                                        for (var i in $scope.exports) {
                                            if ($scope.exports[i].ImportExportID === newStatus[0].ImportExportID) {
                                                $scope.exports[i].ImportExportStatuses = response.data;
                                                break;
                                            }
                                        }

                                        setTimeout(function () {
                                            $scope.message = '';
                                            $state.go('dashboard.exports');
                                        }, 2000);
                                        break;
                                }
                            }
                        }, function (response) {
                            $scope.message = 'Status update failed.';
                            $scope.messageClass = 'text-danger';
                        });
                }
                else {
                    $scope.message = 'All fields are required.';
                    $scope.messageClass = 'text-danger';
                }
            }

            $scope.saveProblemUpdate = function () {
                if ($scope.newProblem.ProblemID !== undefined && $scope.stus.Year !== undefined
                    && $scope.stus.Month !== undefined && $scope.stus.Day !== undefined) {

                    var newProblem = {
                        'ImportExportID': $scope.selectedID,
                        'ProblemID': $scope.newProblem.ProblemID,
                        'ProblemDate': $scope.stus.Year + "/" + $scope.stus.Month + "/" + $scope.stus.Day
                    };

                    $http({
                        method: 'POST',
                        data: newProblem,
                        url: uriProblemUpdates,
                        headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            if (response.data !== undefined) {
                                $scope.message = 'Problem successfully saved.';
                                $scope.messageClass = 'text-info';

                                switch ($sessionStorage.Module) {
                                    case 'import':
                                        // get the index of import object with matching importExportID
                                        // inject problems into the import problems
                                        for (var i in $scope.imports) {
                                            if ($scope.imports[i].ImportExportID === newProblem.ImportExportID) {
                                                $scope.imports[i].ProblemUpdates = response.data;
                                                break;
                                            }
                                        }

                                        setTimeout(function () {
                                            $scope.message = '';
                                            $state.go('dashboard.imports');
                                        }, 2000);
                                        break;
                                    case 'export':
                                        // get the index of export object with matching importExportID
                                        // inject problems into the export problems
                                        for (var i in $scope.exports) {
                                            if ($scope.exports[i].ImportExportID === newProblem.ImportExportID) {
                                                $scope.exports[i].ProblemUpdates = response.data;
                                                break;
                                            }
                                        }

                                        setTimeout(function () {
                                            $scope.message = '';
                                            $state.go('dashboard.exports');
                                        }, 2000);
                                        break;
                                }
                            }
                        }, function (response) {
                            $scope.message = 'Problem update failed.';
                            $scope.messageClass = 'text-danger';
                        });
                }
                else {
                    $scope.message = 'All fields are required.';
                    $scope.messageClass = 'text-danger';
                }
            }

            $scope.imports = [];
            $scope.exports = [];
            $scope.lastImportID = 0;
            $scope.lastExportID = 0;
            $scope.problemsImport = 0;
            $scope.problemsExport = 0;

            // Paging Models and functions
            $scope.pageLimit = 5; //max no of items to display in a page

            // returns the next or previous page
            function getPage(p, currentPage, totalItems) {
                switch (p) {
                    case '-':
                        return currentPage > 1 ? currentPage -= 1 : currentPage;
                    case '+':
                        var lastPage = Math.ceil(totalItems / $scope.pageLimit);
                        return (currentPage < lastPage) ? currentPage += 1 : currentPage;
                }
            }

            // Pager model for import and export
            // contains info like current page and total records.
            $scope.pager = {
                import: {
                    currentPage: 1,
                    totalItems: 0,
                    filteredItems: 0
                },
                export: {
                    currentPage: 1,
                    totalItems: 0,
                    filteredItems: 0
                },
                setPage: function (p, ie) {
                    switch (ie) {
                        case 'import':
                            $scope.pager.import.currentPage =
                                getPage(p, $scope.pager.import.currentPage, $scope.pager.import.totalItems);
                            break;
                        case 'export':
                            $scope.pager.export.currentPage =
                                getPage(p, $scope.pager.export.currentPage, $scope.pager.export.totalItems);
                            break;
                    }
                }
            };

            //$scope.setPage = function (p) {
            //    switch (p) {
            //        case '-':
            //            $scope.currentPage > 1 ? $scope.currentPage -= 1 : '';
            //            break;
            //        case '+':
            //            ($scope.currentPage < Math.ceil($scope.totalItems / $scope.pageLimit)) ? $scope.currentPage += 1 : '';
            //            break;
            //    }
            //};
            $scope.filter = function () {
                $timeout(function () {
                    $scope.filteredItems = $scope.filtered.length;
                }, 10);
            };

            $scope.refresh = function (m) {
                switch (m) {
                    case 'import':
                        $scope.lastImportID = 0;
                        $scope.imports = [];
                        $scope.getImports();
                        break;
                    case 'export':
                        $scope.lastExportID = 0;
                        $scope.exports = [];
                        $scope.getExports();
                }
            };

            function getImports() {
                $http({
                    method: 'GET',
                    url: uriImport + '?companyId=' + $scope.user.Company.CompanyID
                        + '&lastID=' + $scope.lastImportID,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        // merge response.data with $scope.imports
                        $scope.imports = $scope.imports.concat(response.data);

                        $scope.pager.import.filteredItems = $scope.imports.length; //Initially for no filter  
                        $scope.pager.import.totalItems = $scope.imports.length;

                        // set the last import ID
                        $scope.lastImportID = response.data[response.data.length - 1].ID;

                        if ($sessionStorage.ImportExportID != undefined) $scope.viewImportExport($sessionStorage.ImportExportID);

                        for (var i = 0; i < response.data.length; i++) {
                            $scope.problemsImport += response.data[i].ProblemUpdates.length;
                        }

                        // get next chunk of imports
                        $scope.getImports();
                    });
            }

            function getExports() {
                $http({
                    method: 'GET',
                    url: uriExport + '?companyId=' + $scope.user.Company.CompanyID
                        + '&lastID=' + $scope.lastExportID,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.exports = $scope.exports.concat(response.data);

                        $scope.pager.export.filteredItems = $scope.exports.length; //Initially for no filter
                        $scope.pager.export.totalItems = $scope.exports.length;

                        // set the last export ID
                        $scope.lastExportID = response.data[response.data.length - 1].ID;

                        if ($sessionStorage.ImportExportID != undefined) $scope.viewImportExport($sessionStorage.ImportExportID);

                        for (var i = 0; i < response.data.length; i++) {
                            $scope.problemsExport += response.data[i].ProblemUpdates.length;
                        }

                        // get next chunck of exports
                        $scope.getExports();
                    });
            }

            function getDemurrage() {
                $scope.DemurrageData = {
                    ReportName: 'demurragereport',
                    DocumentType: 'all',
                    CompanyID: $sessionStorage.LoginInfoObjs.Company.CompanyID,
                    ConfigStatsTimes: $sessionStorage.configStatsTimes
                };

                $http({
                    method: 'POST', url: uriReports + '/Regular', data: $scope.DemurrageData, headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        if (response.data !== undefined) {
                            $scope.demurrage = response.data;

                            // TODO: this not required anymore for import
                            //$scope.demurrageImport = function () {
                            //    var importDemurrage = [];
                            //    for (var i in $scope.demurrage) {
                            //        if ($scope.demurrage[i].DocType.toLowerCase() == 'import')
                            //            importDemurrage.push($scope.demurrage[i]);
                            //    }

                            //    return importDemurrage;
                            //};

                            // TODO: this not required anymore for export
                            //$scope.demurrageExport = function () {
                            //    var exportDemurrage = [];
                            //    for (var i in $scope.demurrage) {
                            //        if ($scope.demurrage[i].DocType.toLowerCase() == 'export')
                            //            exportDemurrage.push($scope.demurrage[i]);
                            //    }

                            //    return exportDemurrage;
                            //};
                        }
                    });
            }

            //getImportsByBill
            $scope.getImportsByBill = function (t) {
                $http({
                    method: 'GET',
                    url: uriImport + '?companyId=' + $scope.user.Company.CompanyID + '&bol=' + t,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        if (response.data !== undefined && response.data.length > 0) {
                            $scope.imports = response.data;
                            if ($sessionStorage.ImportExportID != undefined) $scope.viewImportExport($sessionStorage.ImportExportID);
                            var c = 0;
                            for (var i = 0; i < $scope.imports.length; i++) {
                                c += $scope.imports[i].ProblemUpdates.length;
                            }

                            $scope.problemsImport = c;
                        }
                    });
            };

            //getExportsBySin
            $scope.getExportsBySin = function (t) {
                $http({
                    method: 'GET',
                    url: uriExport + '?companyId=' + $scope.user.Company.CompanyID + '&sin=' + t,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        if (response.data !== undefined && response.data.length > 0) {
                            $scope.exports = response.data;
                            if ($sessionStorage.ImportExportID != undefined) $scope.viewImportExport($sessionStorage.ImportExportID);
                            var c = 0;
                            for (var i = 0; i < $scope.exports.length; i++) {
                                c += $scope.exports[i].ProblemUpdates.length;
                            }

                            $scope.problemsExport = c;
                        }
                    });
            };


            $scope.getImports = getImports;
            $scope.getExports = getExports;
            $scope.getDemurrage = getDemurrage;

            $scope.getImports(); // get all imports for company
            $scope.getExports(); // get all exports for company
            $scope.getDemurrage(); // get all demurrage for company

            $scope.getDemurrageHighlight = function (days) {
                if (days > 0) return 'bg-color-red';
                if (days > -3) return 'bg-color-orange';
                if (days < 1) return 'bg-color-green';
            }

            // view imports/exports
            $scope.viewImportExport = function (ImportExportID) {
                $scope.formData = { 'ImportExport': {} };
                $sessionStorage.ImportExportID = ImportExportID;
                var index = -1;

                switch ($sessionStorage.Module) {
                    case 'import':
                        for (var i = 0; i < $scope.imports.length; i++) {
                            if ($scope.imports[i].ImportExportID == ImportExportID) {
                                index = i;
                                break;
                            }
                        }

                        $scope.formData.ImportExport = $scope.imports[index];

                        //added code for displaying customs broker for this ImportExport 
                        $scope.getCustomsBrokerClearableForCurrentImportExport($scope.formData.ImportExport);

                        $state.go('dashboard.imports.viewimport');
                        break;
                    case 'export':
                        for (var i = 0; i < $scope.exports.length; i++) {
                            if ($scope.exports[i].ImportExportID == ImportExportID) {
                                index = i;
                                break;
                            }
                        }

                        $scope.formData.ImportExport = $scope.exports[index];
                        $state.go('dashboard.exports.viewexport');
                        break;
                }
            };

            $scope.getProblemNo = function (m) {
                switch (m) {
                    case 'import':
                        var c = 0;
                        for (var i = 0; i < $scope.imports.length; i++) {
                            c += $scope.imports[0].ProblemUpdates.length;
                        }
                        return c;
                }
            }

            $scope.getProblemNotPassed = function (o) {
                //console.log(JSON.stringify(o));
                if (o === undefined || o.length === 0) return $scope.problemNotPassed = false;

                for (var i in o) {
                    switch (o[i].IsResolved) {
                        case false: return $scope.problemNotPassed = true;
                    }
                }

                return $scope.problemNotPassed = false;
            }

            $scope.getStatusNotPassed = function (o) {
                if (o.ImportExportStatuses === undefined
                    || o.ImportExportStatuses.length === 0) return $scope.statusNotPassed = true;

                // check all problems are also closed
                if ($scope.getProblemNotPassed(o.ProblemUpdates) == true) return $scope.statusNotPassed = true;

                var count = 0;
                for (var i in o.ImportExportStatuses) {
                    switch (o.ImportExportStatuses[i].StatusID) {
                        case '18': count++; break;
                        case '8': count++; break;
                        case '9': count++; break;
                        case '10': count++; break;
                        case '11': count++; break;
                        case '12': count++; break;
                        case '13': count++; break;
                        case '57': count++; break;
                    }

                    if (count == 8) return $scope.statusNotPassed = false;
                }

                return $scope.statusNotPassed = true;
            };

            $scope.terminate = function (importExportID) {
                switch ($scope.module) {
                    case 'import':
                        $http({
                            method: 'PUT', url: uriImport + '/Terminated?importExportID=' + importExportID, headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                $scope.getImports();
                                $state.go('dashboard.imports');
                            });
                        break;
                    case 'export':
                        $http({
                            method: 'PUT', url: uriExport + '/Terminated?importExportID=' + importExportID, headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                $scope.getExports();
                                $state.go('dashboard.exports');
                            });
                        break;
                }

                $sessionStorage.ImportExportID = undefined;
            }

            $scope.completeImportExport = function (importExportID) {
                switch ($scope.module) {
                    case 'import':
                        $http({
                            method: 'PUT', url: uriImport + '/Completed?importExportID=' + importExportID, headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                $scope.getImports();
                                $state.go('dashboard.imports');
                            });
                        break;
                    case 'export':
                        $http({
                            method: 'PUT', url: uriExport + '/Completed?importExportID=' + importExportID, headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                $scope.getExports();
                                $state.go('dashboard.exports');
                            });
                        break;
                }

                $sessionStorage.ImportExportID = undefined;
            };

            $scope.trackings = [];
            // tracking numbers for the agent
            $scope.gotoAgentTracking = function () {
                $http({
                    method: 'GET',
                    url: uriTracking + '?companyID=' + $sessionStorage.LoginInfoObjs.Company.CompanyID,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.trackings = response.data;
                        $state.go('dashboard.tracking');
                    });
            };

            $scope.updateTracking = function (tracking) {
                $http({
                    method: 'PUT',
                    url: uriTracking,
                    data: tracking,
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        // show a flash message here
                        //console.log('update done');
                    });
            }

            //CUSTOMS AGENT STARTS HERE ---------------

            $scope.flgAllowChangingCustomsBroker = false;
            $scope.CustomsBrokerClearable = {};
            $scope.CustomsBrokerClearableProcessStatus = "";

            // TODO: need to fix the timeout function
            $scope.hideEditCustomsBrokerClearable = function () {
                $timeout(function () {
                    $scope.flgAllowChangingCustomsBroker = false;
                    //hides the result message i.e updated, failed, etc
                    $scope.CustomsBrokerClearableProcessStatus = "";
                }, 3000);
            }


            $scope.updateCustomsBrokerUserName = function () {
                //update the clearableObj for this ImportExportID
                $scope.flgCCBLoading = true;
                $http.post(uriCustomsBroker + "/UpdateCustomsBrokerForImportExport", $scope.CustomsBrokerClearable)
                    .then(function (response) {
                        if (response.data != null) {
                            var result = 0;
                            result = response.data;
                            if (result > 0) {
                                $scope.CustomsBrokerClearableProcessStatus = "Updated";
                            }
                        }
                        $scope.flgCCBLoading = false;

                        //hides the edit area for Customs Broker username for this ImportExport import object
                        $scope.hideEditCustomsBrokerClearable();

                    }, function (errorResponse) {
                        $scope.CustomsBrokerClearableProcessStatus = "Failed Update:" + errorResponse.data;
                        $scope.flgCCBLoading = false;
                    });
            }

            $scope.getCustomsBrokerClearableForCurrentImportExport = function (ImportExport) {
                //gets the CustomsBrokerClearable from webapi
                $scope.flgCCBLoading = true;
                //console.log(ImportExport.ImportExportID);
                $http.post(uriCustomsBroker + "/" + "CustomsBrokerForImportExport", ImportExport)
                .then(function (response) {
                    if (response.data == null) {
                        //create a new object for editing
                        $scope.CustomsBrokerClearable = {};
                        $scope.CustomsBrokerClearable.importexportid = importexportid;
                        $scope.CustomsBrokerClearable.ownerusername = $sessionStorage.LoginInfoObjs.Login.UserName;
                        $scope.CustomsBrokerClearable.customsbrokerusername = "";
                        $scope.CustomsBrokerClearable.cleared = "NO";
                        $scope.CustomsBrokerClearable.id = "";
                    } else {
                        $scope.CustomsBrokerClearable = response.data
                    }
                    $scope.flgCCBLoading = false;
                }, function (errorResponse) {
                    //console.log("Failed Getting CustomsBroker Object: " + errorResponse.data);
                    $scope.flgCCBLoading = false;
                });
            }

            // CUSTOMS AGENT ENDS HERE ------------------

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

        }]);

    app.controller('importExportController', ['$scope', '$http', '$sessionStorage', '$state', 'refresher',
        function ($scope, $http, $sessionStorage, $state, refresher) {
            //refresher.refreshApp();
            if ($sessionStorage.LoginInfoObjs == undefined) { $state.go("login"); }

            $scope.retainDesc = function () {
                if ($('#Description').length !== 0) {
                    // get the description value with jquery and assign it manually
                    var desc = $('#Description').val();
                    $scope.formData.ImportExport.Items[0].Description = desc;
                }
            }

            $scope.formData = {
                'ImportExport': {
                    'Tracking': {},
                    'ConsigneeID': $scope.user.Person.PersonID,
                    'ReceiverID': $scope.user.Company.CompanyID,
                    'ImpExpTypeID': '',
                    'Items': [
                        {
                            'Description': '',
                            'ItemOrderNo': '',
                            'ItemDetail': {
                                'StatusDate': ''
                            }
                        }
                    ],
                    'ImportExportStatuses': [
                        {
                            'StatusID': '',
                            'StatusDate': ''
                        }
                    ],
                    'DateDischarge': '',
                    'ModeOfTransportID': '',
                    'CarrierID': '',
                    'Cost': {}
                }
            };

            if ($sessionStorage.Tracking)
                $scope.formData.ImportExport.Tracking = $sessionStorage.Tracking;

            $scope.gotoModule = function () {
                switch ($sessionStorage.Module) {
                    case 'import':
                        $state.go('dashboard.imports'); break;
                    case 'export':
                        $state.go('dashboard.exports'); break;
                }

                $sessionStorage.Tracking = undefined;
            };


            //initially sets the customsbrokerusername to the defaultbrokerusername from $sessionStorage obtained during login
            $scope.CustomsBrokerClearable = {};
            $scope.CustomsBrokerClearable.customsbrokerusername = $sessionStorage.LoginInfoObjs.DefaultBroker.defaultusername;

            $scope.processForm = function () {
                $scope.formData.ImportExport.ImportExportStatuses[0].StatusDate = $scope.formData.ImportExport.Items[0].ItemDetail.StatusDate;
                $scope.formData.ImportExport.ImportExportStatuses[0].StatusID = $scope.formData.ImportExport.Items[0].ItemDetail.StatusID;

                switch ($sessionStorage.Module) {
                    case 'import':
                        $scope.formData.ImportExport.BillOfLadingDate = $scope.formData.ImportExport.LoadingDate;
                        $http({
                            method: 'POST',
                            data: $scope.formData.ImportExport,
                            url: uriImport + '/PostImport',
                            headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                if (response.data !== undefined) {
                                    $scope.formData.ImportExport.Tracking = $sessionStorage.Tracking = response.data.Tracking;
                                    // add import to the top of the imports array
                                    $scope.imports.splice(0, 0, response.data.Import);

                                    //assign the importexportid to the importexportid of the import that is beign added
                                    ////using the returned Tracking Information to extract the importexportid is bad design
                                    $scope.CustomsBrokerClearable.importexportid = $scope.formData.ImportExport.Tracking.ImportExportID;
                                    $scope.CustomsBrokerClearable.ownerusername = $sessionStorage.LoginInfoObjs.Login.UserName;
                                    $scope.CustomsBrokerClearable.cleared = "NO";

                                    //console.log("About to call AddCustomsBrokerForImport");
                                    $http.post(uriImport + "/AddCustomsBrokerForImport", $scope.CustomsBrokerClearable)
                                        .then(function () {
                                            $state.go('dashboard.imports.add.tracking');
                                        }, function (error) {
                                            //console.log("Failed Assigning CustomsBroker Clearable Object: " + error.data);
                                        });
                                }
                            });
                        break;
                    case 'export':
                        $http({
                            method: 'POST', data: $scope.formData.ImportExport, url: uriExport, headers: { 'Content-Type': 'application/json' }
                        })
                            .then(function (response) {
                                if (response.data !== undefined) {
                                    $scope.formData.ImportExport.Tracking = $sessionStorage.Tracking = response.data.Tracking;
                                    $scope.exports.splice(0, 0, response.data.Export);
                                    $state.go('dashboard.exports.add.tracking');
                                }
                            });
                        break;
                }
            };

            // load Carrier on MOT
            $scope.formData.ImportExport.loadCarrier = function () {
                $scope.mot.ModeOfTransportID = $scope.formData.ImportExport.ModeOfTransportID.toString();
            };

            // load vessel on CarrierID
            $scope.formData.ImportExport.loadVessel = function () {
                $scope.carrier.CarrierID = $scope.formData.ImportExport.CarrierID.toString();
            };

            // load subCargo on CargoID
            $scope.formData.ImportExport.loadCargo = function (itemIndex) {
                $scope.cargo.CargoID = $scope.formData.ImportExport.Items[itemIndex].CargoID;
            };

            // load ports on CountryID
            $scope.formData.ImportExport.loadPorts = function (isPortOfLoading) {

                var ports = $scope.valuesData.Ports;
                var length = ports.length;
                var portsOfCountry = [];

                function filterPorts(countryID) {
                    for (var i = 0; i < length; i++) {
                        if (ports[i].CountryID == countryID) {
                            portsOfCountry.push(ports[i]);
                        }
                    }
                }

                switch (isPortOfLoading) {
                    case 'true':
                        var countryID = $scope.formData.ImportExport.CountryLoading.CountryID;
                        filterPorts(countryID);

                        if (portsOfCountry != null) {
                            $('#PortOfLoadingID').empty();
                            $('#PortOfLoadingID').append(new Option('', ''));

                            for (var i = 0; i < portsOfCountry.length; i++) {
                                $('#PortOfLoadingID').append(new Option(portsOfCountry[i].PortName, portsOfCountry[i].ID))
                            }
                        }
                        break;
                    default:
                        var countryID = $scope.formData.ImportExport.CountryDischarge.CountryID;
                        filterPorts(countryID);

                        if (portsOfCountry != null) {
                            $('#PortOfDischargeID').empty();
                            $('#PortOfDischargeID').append(new Option('', ''));

                            for (var i = 0; i < portsOfCountry.length; i++) {
                                $('#PortOfDischargeID').append(new Option(portsOfCountry[i].PortName, portsOfCountry[i].ID))
                            }
                        }
                }
            };

            // filtering data - filters
            $scope.mot = { 'ModeOfTransportID': '' };
            $scope.carrier = { 'CarrierID': '' };
            $scope.cargo = { 'CargoID': '' };
            $scope.type = { 'ImpExpTypeID': '1' };
        }]);

    app.controller('reportsController', ['$scope', '$http', '$state', '$sessionStorage', 'refresher',
        function ($scope, $http, $state, $sessionStorage, refresher) {
            refresher.refreshApp();
            if ($sessionStorage.LoginInfoObjs == undefined) { $state.go("login"); }

            // $scope.formData { ReportName:'', CompanyID: '', Queries: [] }
            // this is an angular model that holds information about 
            // the report name, the kind of user based on CompanyID and sql queries which are sent to the Web API
            $scope.formData = {
                'ReportName': '',
                'CompanyID': $sessionStorage.LoginInfoObjs.Company.CompanyID,
                'Queries': [],
                'ConfigStatsTimes': $sessionStorage.configStatsTimes
            };
            $scope.message = '';



            // ------------------------------- START OF DIRECTIVE -------

            //------------------------------------- M A P P I N G   O F   C O L U M N S ---------------
            // CUSTOM REPORTS SECTION
            $scope.dbTables = null;
            $scope.tableColumns = [];
            $scope.column = {};


            $scope.isFirstClause = true;
            $scope.reportQuery = null;
            $scope.queries = [];
            $scope.customReport = [];
            $scope.customReportHeaders = [];
            $scope.currentColumnMapping = {};
            $scope.template = [];

            var conditionID = 0;


            $scope.getTableColumns = function (t, index) {
                $http({
                    method: 'GET', url: uriReports + '/Columns?tableName=' + t, headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.tableColumns[index] = response.data;
                    });
            };

            $scope.refillColumns = function () {
                $scope.tableColumns = [];

                if ($scope.currentColumnMapping == undefined) return;

                var i = 0;
                for (i = 0 ; i < $scope.currentColumnMapping.mappingModelParams.length; i++) {
                    $scope.tableColumns.push(''); // create a blank index
                    $scope.getTableColumns($scope.currentColumnMapping.mappingModelParams[i].tablename, i);
                }
            };

            $scope.getDBTables = function (currentMap) {
                if (currentMap == undefined) {
                    // TODO: $scope.currentColumnMapping getting undefined on --choose column mapping--
                    $scope.currentColumnMapping = { "ColumnMappings": [true, true, true, true, true] };
                }
                else {
                    $scope.currentColumnMapping = currentMap;
                }

                if ($scope.dbTables == null) {
                    $http({
                        method: 'GET', url: uriReports + '/Tables', headers: { 'Content-Type': 'application/json' }
                    })
                        .then(function (response) {
                            $scope.dbTables = response.data;

                            // Then load Columns of the tables in $scope.currentColumnMapping.mappingModelParams[]
                            $scope.refillColumns();
                        });
                }
                else {
                    if (currentMap != undefined) $scope.refillColumns();
                }
            };


            // push a new object to currentColumnMapping.mappingModelParams[]
            $scope.addNewQuery = function () {
                if ($scope.currentColumnMapping.mappingModelParams == undefined) {
                    $scope.currentColumnMapping.mappingModelParams = [];

                    // load tables if dbTables is empty
                    $scope.getDBTables($scope.currentColumnMapping);
                }

                $scope.currentColumnMapping.mappingModelParams.push({});
            };


            $scope.rptQueryBuilder = function (lOp, tbl, col, op, v) {
                if ($scope.reportQuery != null) $scope.isFirstClause = false;
                if (lOp == undefined) lOp = "";
                if (v == undefined) v = "";

                if (op == "LIKE") {
                    $scope.reportQuery = " " + lOp + " " + tbl + "." + col + " " + op + " '%" + v + "%'";
                }
                else {
                    $scope.reportQuery = " " + lOp + " " + tbl + "." + col + " " + op + " '" + v + "'";
                }

                return $scope.reportQuery;
                //add this list of user inputs to the parameters collection
                //the parameters collection is saved in any new custom mapping that is created
                //$scope.addParams(lOp, tbl, col, op, v);
            };


            // Reset all models used to build query including currentMappingParams
            $scope.resetQuery = function () {
                $scope.isFirstClause = true;
                $scope.reportQuery = null;
                $scope.queries = [];

                //clear the mapping model params list
                //$scope.resetParams();
            };

            $scope.resetAllQuery = function () {
                $scope.currentColumnMapping.mappingModelParams = [];
                $scope.resetQuery();
            };

            $scope.submitQuery = function () {
                // build all conditions from $scope.currentColumnMapping.mappingModelParams
                var o = $scope.currentColumnMapping.mappingModelParams;
                for (var i = 0; i < o.length; i++) {
                    var q = o[i];
                    $scope.queries.push($scope.rptQueryBuilder(q.lop, q.tablename, q.columnname, q.op, q.val));
                }

                // send queries to the server
                $http({
                    method: 'POST', url: uriReports + '/Custom', data: $scope.queries, cache: false,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        $scope.customReport = response.data;
                        loadReportHeaders();
                    }, function () {
                        $scope.customReport = [];
                    });

                // reset query
                $scope.resetQuery();
            };

            // hide or show the mapping settings page in customs report 
            $scope.mapIsHidden = true;
            $scope.controlHidden = true;

            $scope.showMappingView = function () {
                $scope.controlHidden = false;
                $scope.mapIsHidden = !$scope.mapIsHidden;

                $scope.getDBTables($scope.currentColumnMapping);
            };

            // ------------------------------- END OF DIRECTIVE -------


            // FETCH REGULAR REPORTS
            $scope.processForm = function () {
                $scope.formData.Queries = [];
                $scope.resetQuery();

                // build all sql conditions based on mappingModelParams
                if ($scope.currentColumnMapping.mappingModelParams != undefined) {
                    var o = $scope.currentColumnMapping.mappingModelParams;
                    for (var i = 0; i < o.length; i++) {
                        var q = o[i];
                        $scope.queries.push($scope.rptQueryBuilder(q.lop, q.tablename, q.columnname, q.op, q.val));
                    }

                    $scope.formData.Queries = $scope.queries;
                }

                $http({
                    method: 'POST', data: $scope.formData, url: uriReports + '/Regular', headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        $scope.reports = response.data;
                        $scope.count = $scope.reports.length;
                        $scope.message = '_RecordsFetched_';
                        $scope.messageClass = 'text-info';

                        // check if it's a status kinda report i.e reportName is a number
                        // put the data in the custom report scopes
                        // this would allow the fields to be generated dynamically using their properties
                        if (!isNaN($scope.formData.ReportName)) {
                            $scope.customReport = response.data;
                            loadReportHeaders();
                        }
                    }, function (error) {
                        switch (error.status) {
                            case 404: $scope.message = '_SearchInvalid_'; break;
                            case -1: $scope.message = '_ConnectionBroken_'; break;
                        }

                        $scope.count = $scope.message = undefined;
                        $scope.messageClass = 'text-danger';
                    });
            };

            // $scope.loadReport()
            // this function loads the report interface
            // by setting the $scope.formData.ReportName
            $scope.loadReport = function (reportName) {
                $scope.message = '';

                if (reportName !== undefined) {
                    $scope.resetAllQuery();
                    $scope.customReport = [];
                    $scope.reports = undefined;
                    $scope.count = undefined;

                    // $scope.formData.ReportName: 
                    // set the Report Name of the formData based on selection
                    // the report name determines what functions the Web API calls
                    $scope.formData.ReportName = reportName;

                    // go to a state based on the selected report name
                    if (isNaN(reportName)) $state.go('dashboard.reports.' + reportName); // check if status
                    else $state.go('dashboard.reports.bystatus');
                }
            };

            $scope.getTotal = function (obj, field) {
                if (obj == undefined) return;
                var total = 0;
                for (var o in obj) {
                    total += obj[o][field];
                }

                return total;
            };

            $scope.getAverage = function (obj, field) {
                if (obj == undefined) return;
                var total = $scope.getTotal(obj, field);
                return total / obj.length;
            };

            // Colors used for styling the reports
            $scope.rowColor = ["bg-color-grey", ""];
            $scope.getRowColor = function (index) {
                return $scope.rowColor[index % 2];
            };

            $scope.ETAs = undefined;

            $scope.getETAs = function () {
                $http({
                    method: 'GET', url: 'Content/data/ETA.json', cache: false, headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        $scope.ETAs = response.data;
                    });
            };

            // get the ETAs from the json file
            $scope.getETAs();

            $scope.getETAFlagAndRowColor = function (origin, ATA, index) {
                // get the ETAs if object does not exist yet
                if ($scope.ETAs == undefined) { $scope.getETAs(); }

                var style = '';

                // extract the country name from origin info
                var country = origin.substr(origin.indexOf(',') + 1).toLowerCase().trim();

                for (var i in $scope.ETAs) {
                    // check for a match between country and the properties of ETA
                    if (country.indexOf(i.toString().trim()) > -1) {
                        // get color based on the ETA for the matched country
                        style = ATA <= $scope.ETAs[i] ? 'color-green' : 'color-red';
                        break;
                    }

                    // compare the ATA with default ETA if no matching country found
                    style = ATA <= $scope.ETAs.default ? 'color-green' : 'color-red';
                }

                // get the row color
                return style += ' ' + $scope.getRowColor(index);
            };

            // USED TO COLOR COMPANY REPORT HEADERS OR RECORDS
            $scope.reportColors = ["report-highlight-white", "report-highlight-red", "report-highlight-green", "report-highlight-orange"];
            $scope.getReportColor = function (index) {
                return $scope.reportColors[index % 4];
            };

            $scope.companyNames = [];
            $scope.getCompanyReportColor = function (company) {
                // push company into companyNames if it doesn't exist
                var index = $scope.companyNames.indexOf(company);
                if (index == -1) $scope.companyNames.push(company);

                // retrieve the class by index of the company in companyNames
                return $scope.reportColors[index % 4] + ' font-9em text-align-left';
            };

            // DEMURRAGE >>>>>>>>>>
            // retrieve corresponding demurrage import
            $scope.fetchImports = function (array, criteria) {
                // used to contruct the rest of the demurrage headers
                if ($scope.customReport.length == 0) {
                    $scope.customReport = array;
                    loadReportHeaders();
                }

                var index = 0;
                for (var i in array) {
                    if (array[i].ImportExportID === criteria) {
                        index = i;
                        return (array[index]);
                    }
                }
            };

            function loadReportHeaders() {
                $scope.customReportHeaders = [];

                for (var o in $scope.customReport[0]) {
                    // reflect through the customReport 
                    // and push the properties to $scope.customReportHeaders
                    // push 'true' into $scope.template
                    $scope.customReportHeaders.push(o);
                    $scope.template.push(true);
                }


                // assign a default mapping of 5 columns if no mapping is selected
                if ($scope.currentColumnMapping == undefined || $scope.currentColumnMapping.ColumnMappings == undefined) {
                    $scope.currentColumnMapping = { ColumnMappings: [true, true, true, true, true] };
                }
            }

        }]);

    app.controller('trackingController', ['$scope', '$http', '$state', '$sessionStorage', 'refresher',
    function ($scope, $http, $state, $sessionStorage, refresher) {
        refresher.refreshApp();
        //if ($sessionStorage.LoginInfoObjs == undefined) { $state.go("login"); }
        if (($sessionStorage.LoginInfoObjs && $sessionStorage.LoginInfoObjs.Company !== null)
            || (!$sessionStorage.LoginInfoObjs && !$sessionStorage.TrackingNumber)) { $state.go("home"); }


        $scope.message = '';
        $scope.trackings = [];

        $scope.getTrackingsByNumber = function () {
            $http({
                method: 'GET', url: uriTracking + '?trackingNumber=' + $sessionStorage.TrackingNumber,
                cache: false, headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    $scope.trackings = response.data;
                    $scope.hideFilter = true;
                });
        };

        $scope.getTrackingsByEmail = function () {
            $http({
                method: 'GET', url: uriTracking + '?email=' + $sessionStorage.LoginInfoObjs.Person.Email,
                cache: false, headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then(function (response) {
                    $scope.trackings = response.data;
                });
        };

        switch ($sessionStorage.TrackingNumber) {
            case undefined:
                $scope.getTrackingsByEmail();
                intervalId = setInterval(function () { $scope.getTrackingsByEmail() }, 600000); // called every 10 minutes
                break;
            default:
                $scope.getTrackingsByNumber();

                if (!$sessionStorage.LoginInfoObjs) {
                    var regTimeoutlID = setTimeout(function () {
                        $('#register-offer').toggle();
                    }, 15000);
                }
                break;
        }

        $scope.customerTrackings = function () {
            $sessionStorage.TrackingNumber = undefined;
            $state.go('tracking');
        }

        $scope.registerCustomer = function () {
            $state.go('registercustomer');
        }

        $scope.cancelOffer = function () {
            $('#register-offer').toggle();
        }

        $scope.parseTimeSpan = function (t) {
            var d = undefined;
            var hh = undefined;
            var MM = undefined;
            var ss = undefined;

            while (ss == undefined) {
                e = t.indexOf(':');
                if (d == undefined) d = parseInt(t.substring(0, e));
                else if (hh == undefined) hh = parseInt(t.substring(0, e));
                else if (MM == undefined) MM = parseInt(t.substring(0, e));
                else if (e == -1) ss = parseInt(t.substring(0, 2));
                t = t.substring(e + 1);
            }

            if (d > 0) {
                return d.toString() + ' days ago';
            }
            else if (hh > 0) {
                return hh == 1 ? 'an hour ago' : hh.toString() + ' hours ago';
            }
            else if (MM > 0) {
                return MM == 1 ? 'a minute ago' : MM.toString() + ' minutes ago';
            }
            else if (ss > 0) {
                return 'few seconds ago';
            }
        };
    }]);

    app.controller('attachmentsController', ['$scope', '$http', '$state', '$sessionStorage', 'refresher',
    function ($scope, $http, $state, $sessionStorage, refresher) {
        refresher.refreshApp();
        if ($sessionStorage.LoginInfoObjs
            && $sessionStorage.LoginInfoObjs.Company.CompanyTypeID != '99') { $state.go("home"); }


        $scope.message = '';
        var x = new Date().toLocaleString();
        alert(x);
        $scope.Emails = [
            {
                From: 'agabeyre@gmail.com',
                Subject: 'Shipments from China',
                Message: 'Hi, Kindly attend to my shipment with bill of lading 234567YU89. Thanks. Haile.'
            },
            {
                From: 'agabeyre@gmail.com',
                Subject: 'Shipments from Dubai',
                Message: 'Hi, Kindly attend to my shipment with bill of lading 324567YU89. Thanks. Boss.'
            },
            {
                From: 'agabeyre@gmail.com',
                Subject: 'Shipments from Australia',
                Message: 'Hi, Kindly attend to my shipment with bill of lading 678967YU89. Thanks. Hail.'
            }
        ];

        //$scope.prep = function () {
        //    for (var i = 0; i < $scope.imports.length; i++) {
        //        var a = $scope.imports[i];
        //        var space = a.ConsigneeName.indexOf(' ');
        //        var found = $scope.emails.indexOf(a.ConsigneeName.substring(0, space) + a.ConsigneeName.substring(space + 1));

        //        if (found == -1) {
        //            $scope.emails.push(a.ConsigneeName);
        //        }
        //    }
        //};

        $scope.attachments = [
            {
                Agent: 'Abdifatah Gabeyre',
                Company: 'Apple Inc.',
                Email: 'test@mail.com',
                DocLinks: [
                    '/docs/bill1.pdf',
                    '/docs/bill2.pdf',
                    '/docs/bill3.pdf'
                ]
            },
            {
                Agent: 'Ade Ade',
                Company: 'Hols Shipping',
                Email: 'mail@mail.com',
                DocLinks: [
                    '/docs/bill1.pdf',
                    '/docs/bill2.pdf',
                    '/docs/bill3.pdf'
                ]
            },
            {
                Agent: 'Mark Polls',
                Company: 'Mark Tilt',
                Email: 'mark@mail.com',
                DocLinks: [
                    '/docs/bill1.pdf',
                    '/docs/bill3.pdf'
                ]
            },
            {
                Agent: 'Kidist Darry',
                Company: 'Kido Movers',
                Email: 'kidist@mail.com',
                DocLinks: [
                    '/docs/bill3.pdf'
                ]
            },
            {
                Agent: 'Ephrem Hamdu',
                Company: 'Matilda Freights Plc.',
                Email: 'ephrem@mail.com',
                DocLinks: [
                    '/docs/bill1.pdf',
                    '/docs/bill2.pdf',
                    '/docs/bill3.pdf',
                    '/docs/bill4.pdf',
                    '/docs/bill5.pdf'
                ]
            }
        ];

    }]);
}());
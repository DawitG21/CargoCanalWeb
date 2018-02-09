//cargoCanalApp.filter('unsafe', function ($sce) {
//    return function (val) {
//        return $sce.trustAsHtml(val);
//    };
//});
app.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
})


.filter('carrier', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.carriers) {
            if ($rootScope.carriers[i].ID == id) {
                return $rootScope.carriers[i].CarrierName;
            }
        }
    };
}])
.filter('country', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.countries) {
            if ($rootScope.countries[i].ID == id) {
                return $rootScope.countries[i].Name;
            }
        }
    };
}])
.filter('danger', function () {
    return function (Dangerous) {
        switch (Dangerous) {
            case true: return '***';
            default: return ''
        }
    };
})
.filter('resolvedtext', function () {
    return function (resolved) {
        switch (resolved) {
            case true: return 'Resolved';
            default: return 'Pending'
        }
    };
})
.filter('inco', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.incoTerms) {
            if ($rootScope.incoTerms[i].ID == id) {
                return $rootScope.incoTerms[i].IncoName;
            }
        }
    };
}])
.filter('location', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.locations) {
            if ($rootScope.locations[i].ID == id) {
                return $rootScope.locations[i].LocationName;
            }
        }
    };
}])
.filter('mot', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.mots) {
            if ($rootScope.mots[i].ID == id) {
                return $rootScope.mots[i].Mode;
            }
        }
    };
}])
.filter('podName', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.pods) {
            if ($rootScope.pods[i].ID == id) {
                return $rootScope.pods[i].PortName;
            }
        }
    };
}])
.filter('polName', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.pols) {
            if ($rootScope.pols[i].ID == id) {
                return $rootScope.pols[i].PortName;
            }
        }
    };
}])
.filter('reason', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.importExportReasons) {
            if ($rootScope.importExportReasons[i].ID == id) {
                return $rootScope.importExportReasons[i].Reason;
            }
        }
    };
}])
 
.filter('stuffMode', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.stuffModes) {
            if ($rootScope.stuffModes[i].ID == id) {
                return $rootScope.stuffModes[i].Description;
            }
        }
    };
}])
.filter('unit', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.units) {
            if ($rootScope.units[i].ID == id) {
                return $rootScope.units[i].UnitName;
            }
        }
    };
}])
.filter('vessel', ['$rootScope', function ($rootScope) {
    return function (id) {
        for (var i in $rootScope.carrierVessels) {
            if ($rootScope.carrierVessels[i].ID == id) {
                return $rootScope.carrierVessels[i].Name;
            }
        }
    };
}])


.filter('problem', function () {
    return function (ProblemID) {
        for (var i = 0; i < problemsList.length; i++) {
            if (problemsList[i].ID == ProblemID) {
                return problemsList[i].Problem;
            }
        }
    };
})


.filter('statusName', function () {
    return function (StatusID) {
        for (var i = 0; i < statusList.length; i++) {
            if (statusList[i].ID == StatusID) {
                return statusList[i].Description;
            }
        }
    };
})

.filter('completed', function () {
    return function (Completed) {
        switch (Completed) {
            case false:
                return '_Pending_';
            case true:
                return '_Done_';
        }
    };
})

.filter('resolved', function () {
    return function (IsResolved) {
        switch (IsResolved) {
            case false:
                return 'N';
            case true:
                return 'Y';
        }
    };
});
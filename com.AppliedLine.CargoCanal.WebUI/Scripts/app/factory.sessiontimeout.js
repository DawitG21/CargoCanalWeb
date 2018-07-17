(function () {
    'use strict';

    app.factory('sessionTimeoutFactory', ['$interval', '$timeout', '$document', '$rootScope', function ($interval, $timeout, $document, $rootScope) {
        var factory = {};

        let handlerModal;

        // initializations 
        const timeOutValue = 40000; // idle time of 40 seconds
        $rootScope.maxSessionCounter = 20;// idle grace period
        $rootScope.displaySessionModal = false;


        const countdown = function () {
            $rootScope.counter = $rootScope.counter - 1;
        };


        const resetCounter = function () {
            $rootScope.counter = angular.copy($rootScope.maxSessionCounter); 
        };


        const showModal = function () {
            $rootScope.$apply(() => {
                $rootScope.displaySessionModal = true;
            });
        };


        const hideModal = function () {
            // $timeout to schedule scope in future stack 
            // and ensure code is called in single $apply block

            $timeout(() => {
                $rootScope.displaySessionModal = false;
            }, 0);
        };


        const clearModalInterval = function () {
            $interval.cancel(handlerModal);
        };


        const logOut = function () {
            hideModal();
            $rootScope.logout();
        };


        const updateCounter = function () {
            if ($rootScope.counter < 0) return;

            countdown();

            if ($rootScope.counter <= 0) {
                // clear modal handler and logout
                clearModalInterval();
                logOut();
            }
        };

        // show modal with countdown
        // attach handler
        const modalCountdown = function () {
            resetCounter();

            handlerModal = $interval(function () {
                updateCounter();
            }, 1000);
        };


        const startCountdown = function () {
            showModal();
            modalCountdown();
        };



        // used to initialize timeout countdown
        factory.timeoutInit = function () {
            $timeout.cancel($rootScope.timeOutSession);

            $rootScope.timeOutSession = $timeout(function () {
                startCountdown(); // initiates the countdown display
            }, timeOutValue);
        };


        function cancelTimeOut(e) {
            if ($rootScope.User) {
                hideModal();

                // clear handlerModal
                clearModalInterval();

                // invoke timeout listener if user is still connected
                factory.timeoutInit();
            }
        }


        var bodyElement = angular.element($document);
        angular.forEach(['keydown', 'keyup', 'mousemove', 'click', 'DOMMouseScroll', 'mousewheel', 'mousedown',
            'touchstart', 'touchmove', 'scroll', 'focus'
        ], function (eventName) {
            bodyElement.bind(eventName, function (e) {
                cancelTimeOut(e);
            });
        });


        return factory;
    }]);

})();
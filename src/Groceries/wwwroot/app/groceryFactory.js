(function () {
    'use strict';

    angular
        .module('groceryApp')
        .factory('groceryFactory', groceryFactory);

    groceryFactory.$inject = ['$http'];

    function groceryFactory($http) {
        var service = {
            getGroceries: getGroceries
        };

        return service;

        function getGroceries() {
            return $http.get('/api/Grocery');
        }
    }
})();
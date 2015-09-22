(function () {
    'use strict';

    angular
        .module('groceryApp')
        .controller('groceryController', ['$scope', '$http', groceryController]);

    function groceryController($scope, $http) {
        //removing factory for now... groceryFactory.getGroceries().success(function (data) {$scope.groceries = data;}).error(function (error) {// log errors         });

        $scope.loading = true;
        $scope.selected = {}; //{Id:-1};

        //$scope.groceries = [];
        $http.get('/api/Grocery').success(function (data) {          
            $scope.groceries = data;  
            $scope.loading = false;  
        }).error(function () {  
            $scope.error = "An Error has occured while loading groceries!";  
            $scope.loading = false;  
        });  

        
        $scope.toggleEdit = function () {  
            this.friend.editMode = !this.friend.editMode; 
        };

        //ADD
        $scope.add = function () {
            $scope.loading = true;
            var newObj = this.newG ;
            $http.post('/api/Grocery/', newObj).success(function (data) {  
                //alert("Added Successfully!! Id=" + data.Id);  
                $scope.groceries.push({ Id: data.Id, Title: newObj.Title, Price: newObj.Price });
                $scope.loading = false;  
            }).error(function (data) {  
                $scope.error = "An Error has occured while Adding Groceries! " + data;  
                $scope.loading = false;  
              });  
        };


        //DELETE
        $scope.deleteGrocery = function () {
            $scope.loading = true;
            var groceryId = this.g.Id;
            $http.delete('/api/Grocery/' + groceryId).success(function (data) {
                //alert("Deleted Successfully!!");
                $.each($scope.groceries, function (i) {
                    if ($scope.groceries[i].Id === groceryId) {
                        $scope.groceries.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Deleting! " + data;
                $scope.loading = false;
            });
        };

        //UPDATE
        $scope.getTemplate = function (g) {
            if (g.Id === $scope.selected.Id) return 'edit';
            else return 'display';
        };

        $scope.startEdit = function (g) {
            $scope.selected = angular.copy(g);
        };

        $scope.save = function (g, idx) {
            $scope.loading = true;
            //alert("Changes persisted" + g.Id + "->" + idx);
            //var obj = this.g;
            $http.put('/api/Grocery/'+ g.Id, g).success(function (data) {
                $scope.loading = false;

            }).error(function (data) {
                $scope.error = "An Error has occured while saving! " + data;
                $scope.loading = false;
            });
            $scope.groceries[idx] = angular.copy($scope.selected);
            $scope.reset();
        };

        $scope.reset = function () {
            $scope.selected = {};
        };

        //Reorder items.... Up/down
        $scope.moveDown = function (g) {
            $http.put('/api/Grocery/move/' + g.Id, true).success(function (data) {
            }).error(function (data) {
                $scope.error = "An Error has occured while moving! " + data;
            });
            window.location.reload();
        }
        $scope.moveUp = function (g) {
            $http.put('/api/Grocery/move/' + g.Id, false).success(function (data) {
            }).error(function (data) {
                $scope.error = "An Error has occured while moving! " + data;
            });
            window.location.reload();
        }

    };
})();

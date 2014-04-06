'use strict';

/* Controllers */

angular.module('myApp.controllers', [])
  .controller('MyCtrl1', [function() {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('MyCtrl3', ['$scope', '$http', function ($scope, $http) {
    $scope.hasSupervisor = function (employee) { return employee.Supervisor; };

    $http({
      url: '../NwndSvc.asmx/GetAllEmployees',
      method: "POST",
      data: "{}",
      headers: { 'Content-Type': 'application/json' }
    }).success(function (data, status, headers, config) {
      $scope.employees = data.d;
    }).error(function (data, status, headers, config) {
      $scope.data = data;
    });
  }]);

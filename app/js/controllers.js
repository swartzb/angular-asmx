'use strict';

/* Controllers */

angular.module('myApp.controllers', [])
  .controller('MyCtrl1', [function() {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$http', function ($scope, $http) {
    $scope.toggleSelect = function (employee) {
      var ndx, len;
      if (!employee.isSelected) {
        for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
          $scope.employees[ndx].isSelected = false;
        };
      };
      employee.isSelected = !employee.isSelected;
      return;
    };

    $http({
      url: '../NwndSvc.asmx/GetAllEmployees',
      method: "POST",
      data: "{}",
      headers: { 'Content-Type': 'application/json' }
    }).success(function (data, status, headers, config) {
      var ndx, len;
      $scope.employees = data.d;
      for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
        $scope.employees[ndx].isSelected = false;
      };
      return;
    }).error(function (data, status, headers, config) {
      $scope.data = data;
    });
  }]);

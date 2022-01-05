app.controller("meeting", ['$scope', '$http', '$rootScope', '$location', function ($scope, $http, $rootScope, $location) {

    var tokenKey = 'accessToken';
    $scope.data = {};
    //var token = sessionStorage.getItem(tokenKey);
    //if (token == null) {
    //    var origin = window.location.origin;
    //    window.location.href = origin + "/Home/Login";
    //}

    $scope.data = {};
    $scope.crmt = true;
    $scope.back = false;
    $scope.next = true;
    $scope.save = false;
    $scope.class = 'text-right';
    $scope.data.meetingInvitesList = [];
    $scope.locationSelects = [{ Id: 1, Name: '1号会议室' }, { Id: 2, Name: '2号会议室' }, { Id: 3, Name: '3号会议室' }, { Id: 4, Name: '4号会议室' }, { Id: 5, Name: '5号会议室' }, { Id: 6, Name: '6号会议室' }, { Id: 7, Name: '7号会议室' }, { Id: 8, Name: '8号会议室' }, { Id: 9, Name: '9号会议室' }];
    $scope.data.locations = [{ Id: 1, Name: '1号会议室' }, { Id: 2, Name: '2号会议室' }, { Id: 3, Name: '3号会议室' }, { Id: 4, Name: '4号会议室' }, { Id: 5, Name: '5号会议室' }, { Id: 6, Name: '6号会议室' }, { Id: 7, Name: '7号会议室' }, { Id: 8, Name: '8号会议室' }, { Id: 9, Name: '9号会议室' }];
    $scope.userSelects = [];
    $scope.TaskAssigns = [];

    $scope.GetBasicData = function () {
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        urlData = '../api/Meeting';
        document.body.style.cursor = 'wait';
        $http({
            method: 'GET',
            url: urlData,
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                document.body.style.cursor = 'default';
                $scope.data.userdata = response.data.USERDATA[0];
                $scope.data.user = response.data.USER;

            }
        }, function (response) {
            console.log(response);
        });
    };

    //For Meeting Start
    $scope.nextClick = function () {
        $scope.$broadcast('show-errors-event');

        if ($scope.newForm.$invalid) {
            swal(
                'Incomplete Form',
                'Please input required data.',
                'error'
              );
            return;
        }
        else {
            $scope.crmt = false; 
            $scope.back = true; 
            $scope.next = false; 
            $scope.save = true; 
            $scope.class = 'flexbox';
            $scope.GetBasicData();
        }
    };


    $scope.invite = function (userSelects) {
        var keepGoing = true;
        var uil = $scope.data.meetingInvitesList.length;
        if (uil == 0) {
            angular.forEach(userSelects, function (u, i) {
                for (var j = 0; j < $scope.data.user.length; j++) {
                    if (u == $scope.data.user[j].Id) {
                        $scope.data.meetingInvitesList[i] = $scope.data.user[j];
                        break;
                    }
                }
            });
        }
        else {
            angular.forEach(userSelects, function (u, i) {
                keepGoing = true;
                for (var j = 0; j < $scope.data.meetingInvitesList.length; j++) {
                    if (u == $scope.data.meetingInvitesList[j].Id) {
                        keepGoing = false;
                        break;
                    }
                    else {
                        keepGoing = true;
                    }
                }
                if (keepGoing) {
                    for (var k = 0; k < $scope.data.user.length; k++) {
                        if (u == $scope.data.user[k].Id) {
                            $scope.data.meetingInvitesList[uil] = $scope.data.user[k];
                            uil += 1;
                            break;
                        }
                    }
                }
            });
        }
    };

    $scope.removeInvites = function (index) {
        $scope.data.meetingInvitesList.splice(index, 1);
        $scope.$apply();
    };

    $scope.editData = function () {
        var id = window.location.search.substr(1).split('&')[0];
        if (id !== "") {
            document.body.style.cursor = 'wait';
            $scope.getDataById(id);
        }
    }

    $scope.getDataById = function (Id) {
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var url = '../api/Meeting/GetEditData';
        $http({
            method: 'GET',
            url: url,
            params: { 'Id': Id },
            headers: headers,
        }).then(function (response) {
            $scope.res = response;
            var obj = response.data;
            if (response.status == 200) {
                $scope.data = response.data.MEETINGDATABYID[0];
                $scope.data.duration = $scope.data.startTime + "  至  " + $scope.data.endTime;
                angular.forEach($scope.data.AgendaList, function (ag, i) {
                    angular.forEach(ag.MeetingTasks, function (mt, j) {
                        mt.taskAssigntoUser = "";
                        angular.forEach(mt.TaskAssigns, function (ta, k) {
                            var l = k + 1;
                            ta.name = l + ". " + ta.UserName;
                            mt.taskAssigntoUser += ta.UserName + ", ";
                        });
                        mt.taskAssigntoUser = mt.taskAssigntoUser.slice(0, -2);
                    });
                });
                angular.forEach($scope.data.meetingInvitesList, function (mil, l) {
                    var i = l + 1;
                    mil.name = i + ". " + mil.UserName;
                });
                document.body.style.cursor = 'default';
            }
        }, function (response) {
            document.body.style.cursor = 'default';
            var obj = response.data;
        });
    }

    $scope.saveMeeting = function (data) {
        if ($scope.data.meetingInvitesList != null && $scope.data.meetingInvitesList != undefined && $scope.data.meetingInvitesList.length > 0) {
            var token = sessionStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }


            console.log($scope.data);



            url = "../api/Meeting/SaveMeeting";
            document.body.style.cursor = 'wait';
            $http({
                method: 'POST',
                url: url,
                data: $scope.data,
                headers: headers,
            }).then(function (response) {
                $scope.res = response;
                var obj = response.data;
                if (response.status == 200) {
                    document.body.style.cursor = 'default';
                    swal(
                        '保存成功',
                        '已成功保存信息',
                        'success'
                      ).then((value) => {
                          document.body.style.cursor = 'wait';
                          window.location.href = 'Meeting';
                      });
                }
            }, function (response) {
                document.body.style.cursor = 'default';
                console.log(response);
                swal(
                      'Error',
                      '信息不能保存',
                      'error'
                    );
            });
        } else {
            swal(
                 '请至少邀请一个用户',
                  '',
                  'error'
                 );
        }
    }
    //For Meeting End

    //For Agenda Start
    $scope.addAgenda = function (AgendaItem) {
        if (AgendaItem != null) {
            $scope.newAgenda = {
                vAgendaName: AgendaItem
            };
            if ($scope.data.AgendaList == undefined) {
                $scope.data.AgendaList = [];
            }
            $scope.data.AgendaList.push($scope.newAgenda);
            $scope.addTask($scope.data.AgendaList.length - 1);
            $scope.addDecision($scope.data.AgendaList.length - 1);
            $scope.AgendaItem = null;
        }
        else
            swal(
                 'Please Give Agenda Title',
                  '',
                  'error'
                 );
    };

    $scope.removeAgenda = function (index) {
        swal({
            title: "你确定吗?",
            text: "一旦删除，您将无法恢复此文件!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $scope.data.AgendaList.splice(index, 1);
                $scope.$apply();
                swal(
                    'Successfully Deleted',
                    'Agenda Delete Successfully',
                    'success'
                    );
            } else {
                //swal("Your file is safe!");
            }
        });
    };

    /*------------For Task-------------*/
    $scope.getInvitedUser = function (Id) {
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var url = '../api/Meeting/GetInvitedUser';
        $http({
            method: 'GET',
            url: url,
            params: { 'Id': Id },
            headers: headers,
        }).then(function (response) {
            $scope.res = response;
            var obj = response.data;
            if (response.status == 200) {
                $scope.invitedUser = response.data.INVITEDUSER;
            }
        }, function (response) {
            var obj = response.data;
        });
    }

    $scope.addTask = function (index) {
        $scope.newTask = {};
        if ($scope.data.AgendaList[index].MeetingTasks == undefined) {
            $scope.data.AgendaList[index].MeetingTasks = [];
        }
        $scope.data.AgendaList[index].MeetingTasks.push($scope.newTask);
    };

    $scope.removeTask = function (agindex, tindex) {
        $scope.data.AgendaList[agindex].MeetingTasks.splice(tindex, 1);
    };

    $scope.assign = function (agi, tsi, users) {
        $scope.data.AgendaList[agi].MeetingTasks[tsi].taskAssigntoUser = "";
        angular.forEach(users, function (su, j) {
            $scope.data.AgendaList[agi].MeetingTasks[tsi].taskAssigntoUser += su.UserName + ", ";
        });
        $scope.data.AgendaList[agi].MeetingTasks[tsi].taskAssigntoUser = $scope.data.AgendaList[agi].MeetingTasks[tsi].taskAssigntoUser.slice(0, -2);
    }

    /*-------------For Decision------------*/
    $scope.addDecision = function (index) {
        $scope.newDecision = {};
        if ($scope.data.AgendaList[index].Decisions == undefined) {
            $scope.data.AgendaList[index].Decisions = [];
        }
        $scope.data.AgendaList[index].Decisions.push($scope.newDecision);
    };

    $scope.removeDecision = function (agindex, dindex) {
        $scope.data.AgendaList[agindex].Decisions.splice(dindex, 1);
    };


    $scope.saveAgenda = function (data) {
        //if ($scope.data.meetingInvitesList != null && $scope.data.meetingInvitesList != undefined && $scope.data.meetingInvitesList.length > 0) {
            var token = sessionStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            url = "../api/Meeting/SaveAgenda";
            document.body.style.cursor = 'wait';
            $http({
                method: 'POST',
                url: url,
                data: $scope.data,
                headers: headers,
            }).then(function (response) {
                $scope.res = response;
                var obj = response.data;
                if (response.status == 200) {
                    document.body.style.cursor = 'default';
                    swal(
                        '保存成功',
                        '已成功保存信息',
                        'success'
                      ).then((value) => {
                          document.body.style.cursor = 'wait';
                          window.location.href = 'Meeting';
                      });
                }
            }, function (response) {
                document.body.style.cursor = 'default';
                console.log(response);
                swal(
                      'Error',
                      'Information not save',
                      'error'
                    );
            });
    }
    //For Agenda End

    $scope.ConfirmDialog = function (data) {
        swal({
            title: "你确定吗?",
            text: "一旦删除，您将无法恢复此文件!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $scope.deleteData(data);
            } else {
                //swal("Your file is safe!");
            }
        });
    }

    $scope.deleteData = function (data) {

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        url = "../api/Meeting/RemoveData";
        document.body.style.cursor = 'wait';
        $http({
            method: 'POST',
            url: url,
            params: { 'id': data },
            headers: headers,
        }).then(function (response) {
            if (response.status == 200) {
                document.body.style.cursor = 'default';
                swal(
                  '',
                  'Information Delete Successfully',
                  'success'
                ).then((value) => {
                    document.body.style.cursor = 'wait';
                    window.location.href = 'Meeting';
                });
            }
        }, function (response) {
            document.body.style.cursor = 'default';
            console.log(response);
            var obj = response.data;
            if (obj == "You can't delete this") {
                swal(
                    'Error',
                    'You can not delete this data because related data exist.',
                    'error'
                  );
            }
        });
    };

}]);


(function () {
    
    function getUserId() {
        var userId = localStorage.getItem("surveyUserId");
        if (!userId) {
            userId = Math.trunc(Math.random() * 1000000000).toString() + Date.now().toString();
            localStorage.setItem("surveyUserId", userId);
        }
        return userId;
    }

    var API = {
        getItemsCheckedByCurrentUser: function () {
            return fetch("./api/survey/checked-by-user?userId=" + getUserId())
                .then(resp => resp.json());
        },

        getResults: function () {
            return fetch("./api/survey/results")
                .then(resp => resp.json());
        },

        sendVote: function (surveyItemId, isChecked) {
            return fetch("./api/survey/vote", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ surveyItemId, isChecked, userId: getUserId() })
            });
        }
    };

    var SurveyChart = {
        chart: null,
        initialData: [
            { id: "Java", score: 0 },
            { id: "Scala", score: 0 },
            { id: ".NET", score: 0 },
            { id: "Python", score: 0 },
            { id: "Go", score: 0 },
            { id: "JS", score: 0 },
            { id: "CSS", score: 0 },
            { id: "SQL", score: 0 },
            { id: "Kafka", score: 0 },
            { id: "ML", score: 0 },
            { id: "QA", score: 0 },
            { id: "BA", score: 0 },
            { id: "SA", score: 0 },
        ],
        order: {},

        init: function () {
            for (var i = 0; i < this.initialData.length; i++) {
                var item = this.initialData[i];
                SurveyChart.order[item.id] = i;
            }

            var ctx = document.getElementById('survey-results-chart');
            SurveyChart.chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: SurveyChart.initialData.map(x => x.id),
                    datasets: [{
                        label: 'Заинтересованность участников митапа',
                        data: SurveyChart.initialData.map(x => x.score),
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },

        updateAll: function (data) {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var chartIndex = SurveyChart.order[item.surveyItemId];
                if (chartIndex !== undefined) {
                    SurveyChart.chart.data.datasets[0].data[chartIndex] = item.score;
                }
            }
            SurveyChart.chart.update();
        },

        updateItem: function (surveyItemId, isChecked) {
            var i = SurveyChart.order[surveyItemId];
            if (i !== undefined) {
                if (isChecked) {
                    SurveyChart.chart.data.datasets[0].data[i] += 1
                }
                else {
                    SurveyChart.chart.data.datasets[0].data[i] -= 1
                }
                SurveyChart.chart.update();
            }
        }
    };

    var Checkboxes = {
        init: function () {    
            document.querySelectorAll(".survey-option").forEach(function (x) {
                x.addEventListener("click", function (e) {
                    var checkbox = e.target;
                    var surveyItemId = e.target.id;
                    var isChecked = e.target.checked;
    
                    SurveyChart.updateItem(surveyItemId, isChecked);
    
                    checkbox.disabled = true;
    
                    API.sendVote(surveyItemId, isChecked)
                        .then(() => x.disabled = false)
                });
            });
        },

        setCheckedItems: function (ids) {
            for (var i = 0; i < ids.length; i++) {
                var checkbox = document.getElementById(ids[i]);
                if (checkbox) {
                    checkbox.checked = true;
                }
            }            
        }
    }

    // Инициализация приложения

    Promise.all([
        API.getResults(),
        API.getItemsCheckedByCurrentUser(),
    ]).then(([results, checkedItems]) => {
        document.getElementById("loading").style.display = "none";
        document.getElementById("survey").style.display = "flex";

        SurveyChart.init();
        Checkboxes.init();

        SurveyChart.updateAll(results);
        Checkboxes.setCheckedItems(checkedItems);
    });
})();






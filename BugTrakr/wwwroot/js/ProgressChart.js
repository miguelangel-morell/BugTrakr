var ctx = document.getElementById('myChart').getContext('2d');
var approval = document.getElementById('approval').value;
var progress = document.getElementById('progress').value;
var done = document.getElementById('done').value;
var completed = document.getElementById('completed').value;

var myChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
        datasets: [{
            label: '# of Votes',
            data: [approval, 19, 3, 5, 2, 3],
backgroundColor: [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
],
    borderColor: [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
    ],
        borderWidth: 1
            }]
        },
options: {
    legend: {
        display: false
    },
    maintainAspectRatio: false,
        responsive: false
}
    });
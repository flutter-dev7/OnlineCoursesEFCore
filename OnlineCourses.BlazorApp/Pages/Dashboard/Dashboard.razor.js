window.renderDashboardCharts = (revenue, published, total) => {
    const ctxR = document.getElementById('revenueChart');
    if (ctxR) {
        // Очищаем старый график перед созданием нового (важно для Blazor)
        if (window.myRevenueChart) window.myRevenueChart.destroy();

        window.myRevenueChart = new Chart(ctxR, {
            type: 'line',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Revenue',
                    data: [1000, 2500, 1800, 3200, 2100, revenue],
                    borderColor: '#6366F1',
                    backgroundColor: 'rgba(99, 102, 241, 0.1)',
                    fill: true,
                    tension: 0.4
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    const ctxD = document.getElementById('courseDonut');
    if (ctxD) {
        if (window.myDonutChart) window.myDonutChart.destroy();
        window.myDonutChart = new Chart(ctxD, {
            type: 'doughnut',
            data: {
                labels: ['Published', 'Draft'],
                datasets: [{
                    data: [published, total - published],
                    backgroundColor: ['#ffffff', 'rgba(255,255,255,0.3)'],
                    borderWidth: 0
                }]
            },
            options: { cutout: '80%', plugins: { legend: { display: false } } }
        });
    }
};
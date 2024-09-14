document.addEventListener('DOMContentLoaded', function () {
    var startDateInput = document.getElementById('startDate');
    var endDateInput = document.getElementById('endDate');

    // Initial dates setup
    var today = new Date().toISOString().split('T')[0];
    var oneWeekAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];

    startDateInput.value = oneWeekAgo;
    endDateInput.value = today;

    updateDisplayedDates();

    [startDateInput, endDateInput].forEach(function (input) {
        input.addEventListener('change', updateDisplayedDates);
    });

    function updateDisplayedDates() {
        var startDateValue = startDateInput.value;
        var endDateValue = endDateInput.value;

        if (!startDateValue || !endDateValue) {
            alert('Please fill in both start and end dates');
            return;
        }

        var startDate = new Date(startDateValue);
        var endDate = new Date(endDateValue);

        if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
            alert('Invalid date format');
            return;
        }

        if (startDate > endDate) {
            alert('Start date must be less or equal to end date');
            startDateInput.value = oneWeekAgo;
            endDateInput.value = today;
            updateDisplayedDates();
            return
        }

        console.log('Start Date:', startDateValue);
        console.log('End Date:', endDateValue);
        console.log('Start Date Object:', startDate);
        console.log('End Date Object:', endDate);

       
        var allDates = document.querySelectorAll('.date-item');
        allDates.forEach(function (date) {
            date.classList.remove('d-block');
            date.classList.add('d-none');
        });

        var dates = generateDates(startDate, endDate);

        console.log('Generated Dates:', dates);

        dates.forEach(function (date) {
            var stringDate = formatDate(new Date(date));
            var element = document.getElementById(stringDate);

            if (element) {
                element.classList.remove('d-none');
                element.classList.add('d-block');
            }
        });
    }

    function generateDates(startDate, endDate) {
        var dates = [];
        var currentDate = new Date(startDate);

        while (currentDate <= endDate) {
            dates.push(new Date(currentDate));
            currentDate.setDate(currentDate.getDate() + 1);
        }

        return dates;
    }

    function formatDate(date) {
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0');
        var year = date.getFullYear();

        return `${day}.${month}.${year}`;
    }
});

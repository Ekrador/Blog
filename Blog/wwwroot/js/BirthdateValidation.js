    function GetBirthdayDateLimits() {
        let dateForm = document.getElementById("birthdate");
        const date = new Date();
        let currentDay = String(date.getDate()).padStart(2, '0');

        let currentMonth = String(date.getMonth() + 1).padStart(2, "0");

        let currentYear = date.getFullYear();

        // min age for registration 14 years old
        dateForm.min = `${currentYear - 100}-${currentMonth}-${currentDay}`;
        dateForm.max = `${currentYear - 14}-${currentMonth}-${currentDay}`;
    }
    onload = GetBirthdayDateLimits();

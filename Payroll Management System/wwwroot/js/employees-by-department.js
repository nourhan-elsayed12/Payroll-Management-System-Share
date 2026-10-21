// Cascading dropdown: department -> employees of that department.
// Markup: the department <select> has data-employees-target="#EmployeeId",
// and the employee <select> has data-url pointing to the JSON endpoint.
(function () {
    document.querySelectorAll('select[data-employees-target]').forEach(function (department) {
        var employee = document.querySelector(department.dataset.employeesTarget);
        var placeholder = employee.options[0].cloneNode(true); // "-- اختر الموظف --"

        department.addEventListener('change', async function () {
            employee.replaceChildren(placeholder.cloneNode(true));
            employee.disabled = !department.value;
            if (!department.value) return;

            var response = await fetch(employee.dataset.url + '?departmentId=' + encodeURIComponent(department.value));
            if (!response.ok) return;

            var items = await response.json();
            items.forEach(function (item) {
                employee.add(new Option(item.name, item.id));
            });
        });

        // On first load (or after a failed submit) keep employees locked until a department is chosen
        employee.disabled = !department.value;
    });
})();

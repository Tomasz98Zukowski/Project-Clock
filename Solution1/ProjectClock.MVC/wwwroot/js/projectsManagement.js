document.addEventListener('DOMContentLoaded', function () {
    function filterProjects() {
        var checkboxes = document.querySelectorAll('.organization-checkbox');
        var projects = document.querySelectorAll('.project');

        projects.forEach(function (project) {
            var org = project.getAttribute('data-organization');
            var showProject = Array.from(checkboxes).some(function (checkbox) {
                return checkbox.value === org && checkbox.checked;
            });

            if (showProject) {
                project.style.display = 'block';
            } else {
                project.style.display = 'none';
            }
        });
    }

  
    var checkboxes = document.querySelectorAll('.organization-checkbox');
    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', filterProjects);
    });

  
    filterProjects();
});
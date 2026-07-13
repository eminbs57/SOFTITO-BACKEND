// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Global Table Search Functionality
document.addEventListener('DOMContentLoaded', function() {
    const searchInputs = document.querySelectorAll('.table-search-input');
    
    searchInputs.forEach(input => {
        input.addEventListener('keyup', function() {
            const searchTerm = this.value.toLowerCase();
            const targetTableId = this.getAttribute('data-target');
            
            // Find specific table or the first table on page
            const table = targetTableId ? document.getElementById(targetTableId) : document.querySelector('.table-responsive table');
            
            if (!table) return;
            
            const rows = table.querySelectorAll('tbody tr:not(.no-search)');
            
            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                if (text.includes(searchTerm)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        });
    });
});

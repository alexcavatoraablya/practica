document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('createProductForm');

    form.addEventListener('submit', () => {
        savePhotos();
    });

    const nameInput = document.getElementById("Name");
    const slugInput = document.getElementById("Slug");

    nameInput.addEventListener("input", () => {

        slugInput.value = slugify(nameInput.value, {
            lower: true,
            strict: true,
            trim: true
        });

    });


    tinymce.init({
        selector: '#description',
        plugins: 'advlist autolink link image lists charmap preview anchor pagebreak searchreplace wordcount code fullscreen insertdatetime media table help',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | preview fullscreen',
        menubar: 'file edit view insert format tools table help'
    });

    new Sortable(document.getElementById('imageList'), {
        animation: 150,
        ghostClass: 'opacity-50',
        onEnd: renumberBadges
    });

    const fileInput = document.getElementById('fileInput');
    const imageList = document.getElementById('imageList');

    fileInput.addEventListener('change', handleFiles);

    function handleFiles(event) {
        const files = event.target.files;

        Array.from(files).forEach(file => {
            if (!file.type.startsWith('image/')) return;

            const col = document.createElement('div');
            col.className = 'col';

            const wrapper = document.createElement('div');
            wrapper.className = 'position-relative';
            wrapper.innerHTML = `
        <div class="d-flex align-items-center justify-content-center bg-light rounded border" style="height:100px;">
            <div class="spinner-border spinner-border-sm text-secondary" role="status"></div>
        </div>`;

            col.appendChild(wrapper);
            imageList.appendChild(col);

            const reader = new FileReader();
            reader.onload = function (e) {
                wrapper.innerHTML = '';

                const img = document.createElement('img');
                img.src = e.target.result;
                img.className = 'img-fluid rounded border w-100';
                img.style.height = '100px';
                img.style.objectFit = 'cover';
                img.alt = file.name;
                img.title = file.name;

                const badge = document.createElement('span');
                badge.className = 'badge bg-dark position-absolute bottom-0 start-0 m-1 priority-badge';

                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.className = 'btn btn-sm btn-danger position-absolute top-0 end-0 m-1 rounded-circle';
                removeBtn.innerHTML = '&times;';
                removeBtn.setAttribute('aria-label', 'Видалити фото');
                removeBtn.addEventListener('click', () => {
                    col.remove();
                    renumberBadges();
                });

                wrapper.appendChild(img);
                wrapper.appendChild(badge);
                wrapper.appendChild(removeBtn);

                renumberBadges();
            };
            reader.readAsDataURL(file);
        });

        fileInput.value = '';
    }

    function renumberBadges() {
        imageList.querySelectorAll('.priority-badge').forEach((badge, index) => {
            badge.textContent = index + 1;
        });
    }

    function savePhotos() {
        form.querySelectorAll('input[name^="Images"]').forEach(el => el.remove());

        const imgs = imageList.querySelectorAll('img');

        imgs.forEach((img, index) => {
            const hiddenName = document.createElement('input');
            hiddenName.type = 'hidden';
            hiddenName.name = `Images[${index}].Name`;
            hiddenName.value = img.src;
            form.appendChild(hiddenName);

            const hiddenPriority = document.createElement('input');
            hiddenPriority.type = 'hidden';
            hiddenPriority.name = `Images[${index}].Priority`;
            hiddenPriority.value = index;
            form.appendChild(hiddenPriority);
        });
    }
});

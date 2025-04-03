document.addEventListener('DOMContentLoaded', function () {
    fetchProducts();
    document.getElementById('btnAdd').addEventListener('click', addProduct);
    document.getElementById('btnReset').addEventListener('click', resetForm);
});

function fetchProducts() {
    const apiUrl = 'http://localhost:5076/api/products'; // Thay bằng URL backend của bạn
    fetch(apiUrl)
        .then(handleResponse)
        .then(data => displayProducts(data))
        .catch(error => console.error('Fetch error:', error.message));
}

function handleResponse(response) {
    if (!response.ok) throw new Error('Network response was not ok');
    return response.json();
}

function displayProducts(products) {
    const productList = document.getElementById('productList');
    productList.innerHTML = '';
    products.forEach(product => {
        productList.innerHTML += createProductRow(product);
    });
}

function createProductRow(product) {
    return `
        <tr>
            <td>${product.id}</td>
            <td>${product.name}</td>
            <td>${product.price}</td>
            <td>${product.description}</td>
            <td>
                <button class="btn btn-primary view-btn mr-btn" data-id="${product.id}">Xem</button>
                <button class="btn btn-warning edit-btn mr-btn" data-id="${product.id}">Sửa</button>
                <button class="btn btn-danger delete-btn" data-id="${product.id}">Xóa</button>
            </td>
        </tr>
    `;
}

function addProduct() {
    const productData = {
        name: document.getElementById('bookName').value,
        price: parseFloat(document.getElementById('price').value),
        description: document.getElementById('description').value
    };

    fetch('http://localhost:5076/api/products', { // Thay bằng URL backend của bạn
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(productData)
    })
        .then(handleResponse)
        .then(data => {
            console.log('Product added:', data);
            fetchProducts();
            resetForm();
        })
        .catch(error => console.error('Error:', error));
}

function resetForm() {
    document.getElementById('studentForm').reset();
    document.getElementById('btnUpdate').style.display = 'none';
    document.getElementById('btnAdd').style.display = 'inline';
    document.getElementById('btnClear').style.display = 'none';
}

document.addEventListener('click', function (e) {
    const target = e.target;
    const id = target.getAttribute('data-id');

    if (target.classList.contains('view-btn')) {
        viewProduct(id);
    } else if (target.classList.contains('edit-btn')) {
        editProduct(id);
    } else if (target.classList.contains('delete-btn')) {
        deleteProduct(id);
    }
});

function viewProduct(id) {
    fetch(`http://localhost:5076/api/products/${id}`) // Thay bằng URL backend của bạn
        .then(handleResponse)
        .then(product => {
            const modal = new bootstrap.Modal(document.getElementById('modalViewDetailInfo'));
            document.querySelector('.fullName').textContent = product.name;
            document.querySelector('.code').textContent = product.id;
            document.querySelector('.dateOfBirth').textContent = product.name;
            document.querySelector('.gender').textContent = product.description;
            modal.show();
        })
        .catch(error => console.error('Error:', error));
}

function editProduct(id) {
    fetch(`http://localhost:5076/api/products/${id}`) // Thay bằng URL backend của bạn
        .then(handleResponse)
        .then(product => {
            document.getElementById('bookName').value = product.name;
            document.getElementById('price').value = product.price;
            document.getElementById('description').value = product.description;

            document.getElementById('btnUpdate').style.display = 'inline';
            document.getElementById('btnAdd').style.display = 'none';
            document.getElementById('btnClear').style.display = 'inline';

            document.getElementById('btnUpdate').onclick = function () {
                updateProduct(id);
            };
        })
        .catch(error => console.error('Error:', error));
}

function updateProduct(id) {
    const updatedProduct = {
        id: id,
        name: document.getElementById('bookName').value,
        price: parseFloat(document.getElementById('price').value),
        description: document.getElementById('description').value
    };

    fetch(`http://localhost:5076/api/products/${id}`, { // Thay bằng URL backend của bạn
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedProduct)
    })
        .then(response => {
            if (response.status === 204) {
                console.log('Product updated successfully.');
                fetchProducts();
                resetForm();
            } else {
                throw new Error('Failed to update product');
            }
        })
        .catch(error => console.error('Error:', error));
}

function deleteProduct(id) {
    if (confirm('Bạn có chắc muốn xóa sản phẩm này?')) {
        fetch(`http://localhost:5076/api/products/${id}`, { // Thay bằng URL backend của bạn
            method: 'DELETE'
        })
            .then(response => {
                if (response.status === 204) {
                    console.log('Product deleted successfully.');
                    fetchProducts();
                } else {
                    throw new Error('Failed to delete product');
                }
            })
            .catch(error => console.error('Error:', error));
    }
}
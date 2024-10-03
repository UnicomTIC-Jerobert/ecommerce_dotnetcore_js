const apiUrl = 'http://localhost:5001/api/products'; // Your API endpoint

// Get elements
const createProductBtn = document.getElementById('createProductBtn');
const productModal = document.getElementById('productModal');
const productForm = document.getElementById('productForm');
const closeModal = document.getElementsByClassName('close')[0];
const productTableBody = document.getElementById('productTableBody');
const productIdInput = document.getElementById('productId');
const productNameInput = document.getElementById('productName');
const priceInput = document.getElementById('price');
const descriptionInput = document.getElementById('description');
const imageURLInput = document.getElementById('imageURL');
const saveBtn = document.getElementById('saveBtn');

// Function to show modal
function openModal() {
    productModal.style.display = 'block';
}

// Function to close modal
function closeModalWindow() {
    productModal.style.display = 'none';
    productForm.reset(); // Clear form
    productIdInput.value = ''; // Clear hidden input
}

// Handle modal close
closeModal.onclick = () => closeModalWindow();
window.onclick = (event) => {
    if (event.target === productModal) {
        closeModalWindow();
    }
};

// Handle form submission for Create/Update
productForm.onsubmit = async (e) => {
    e.preventDefault();
    const productId = productIdInput.value;
    const product = {
        productName: productNameInput.value,
        price: parseFloat(priceInput.value),
        description: descriptionInput.value,
        imageURL: imageURLInput.value
    };

    //console.log(JSON.stringify(product))

    if (productId) {
        // Update product
        await updateProduct(productId, product);
    } else {
        // Create product
        await createProduct(product);
    }

    closeModalWindow();
    await loadProducts();
};

// Function to load products and display in table
async function loadProducts() {
    const response = await fetch(apiUrl);
    const products = await response.json();
    productTableBody.innerHTML = '';

    products.forEach((product) => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${product.productId}</td>
            <td>${product.productName}</td>
            <td>${product.price}</td>
            <td>${product.description}</td>
            <td>${product.imageURL}</td>
            <td>
                <button onclick="editProduct(${product.productId})">Edit</button>
                <button onclick="deleteProduct(${product.productId})">Delete</button>
            </td>
        `;
        productTableBody.appendChild(row);
    });
}

// Function to create product
async function createProduct(product) {
    await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    });
}

// Function to update product
async function updateProduct(productId, product) {
    await fetch(`${apiUrl}/${productId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    });
}

// Function to delete product
async function deleteProduct(productId) {
    await fetch(`${apiUrl}/${productId}`, {
        method: 'DELETE'
    });
    await loadProducts();
}

// Function to edit product (populate form)
async function editProduct(productId) {
    const response = await fetch(`${apiUrl}/${productId}`);
    const product = await response.json();

    productIdInput.value = product.productId;
    productNameInput.value = product.productName;
    priceInput.value = product.price;
    descriptionInput.value = product.description;
    imageURLInput.value = product.imageURL;

    openModal();
}

// Open modal for creating a new product
createProductBtn.onclick = () => openModal();

// Load products when page loads
window.onload = async () => {
    await loadProducts();
};

<template>
	<div class="grid">
		<div class="col-12">
			<div class="card">
				<Toast/>
				<Toolbar class="mb-4">
					<template v-slot:start>
						<div class="my-2">
							<Button label="New" icon="pi pi-plus" class="p-button-success mr-2" @click="openNew" />
						</div>
					</template>
					<template v-slot:end>
						<Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($sale)" />
					</template>
				</Toolbar>

                <DataTable ref="dt" :value="items" v-model:selection="selectedSales" dataKey="saleId" :paginator="true" :rows="10" :filters="filters" paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,25]" currentPageReportTemplate="Showing {first} to {last} of {totalRecords} items" responsiveLayout="scroll">
                    <template #header>
                        <div class="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
                            <h5 class="m-0">Sales</h5>
                            <span class="block mt-2 md:mt-0 p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['global'].value" placeholder="Search..." />
                            </span>
                        </div>
                    </template>

                    <Column field="saleId" header="Id" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Id</span>
                            {{slotProps.data.saleId}}
                        </template>
                    </Column>

                    <Column field="fullname" header="Customer" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Customer</span>
                            {{slotProps.data.customer.fullName}}
                        </template>
                    </Column>

                    <Column field="identification" header="Identification" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Identification</span>
                            {{slotProps.data.customer.identification}}
                        </template>
                    </Column>

                    <Column field="email" header="Email" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Email</span>
                            {{slotProps.data.customer.email}}
                        </template>
                    </Column>

                    <Column field="phone" header="Phone" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Phone</span>
                            {{slotProps.data.customer.phone}}
                        </template>
                    </Column>
                    <Column field="isEnabled" header="Active" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Active</span>
                            <InputSwitch v-model="slotProps.data.isEnabled" disabled />
                        </template>
                    </Column>
                    <Column headerStyle="min-width:10rem;">
                        <template #body="slotProps">
                            <Button icon="pi pi-pencil" class="p-button-success mr-2" @click="edit(slotProps.data)" />
                            <Button icon="pi pi-trash" class=" p-button-warning mt-2" v-show="slotProps.data.isEnabled != false" @click="deleteItem(slotProps.data)" />
                        </template>
                    </Column>
                </DataTable>

                <Dialog v-model:visible="saleDialog" :style="{width: '980px'}" header="Sale Details" :modal="true" class="p-fluid">
                    <div class="field" v-if="sale.saleId>0">
                        <h2 class="text-900  mb-2 align-items-center"> {{sale.customer.fullName}}</h2>
                        <hr />
                    </div>
                    <div class="field" v-else>
                        <label for="name">Customer</label>
                        <Dropdown id="name" v-model="customer" :options="customers" optionLabel="fullName" placeholder="Select One" required="true" autofocus :class="{'p-invalid': submitted && !customer}"></Dropdown>
                        <small class="p-invalid" v-if="submitted && !customer">Name is required.</small>
                    </div>
                    <div class="field grid justify-content-end">
                        <div class="field col">
                            <label>Line Sale </label>
                        </div>
                        <div class="field col">
                            <h2 class="text-900  mb-2 align-items-center">Total: {{total}}</h2>
                            <hr />
                        </div>
                    </div>
                    <div class="formgrid grid">
                        <div class="field col">
                            <label for="productId">Product</label>
                            <Dropdown id="productId" v-model="product" :options="products" optionLabel="name" placeholder="Select One" required="true" autofocus :class="{'p-invalid': submitted && !product}"></Dropdown>
                            <small class="p-invalid" v-if="submitted && !product">Product is required.</small>
                        </div>
                        <div class="field col">
                            <label for="unitPrice">Unit Price</label>
                            <InputNumber v-model="product.unitPrice" mode="decimal" :minFractionDigits="2" required="true" autofocus :class="{'p-invalid': submitted && !product.unitPrice}" />
                            <small class="p-invalid" v-if="submitted && !product.unitPrice">Unit Price is required.</small>
                        </div>
                        <div class="field col col">
                            <label for="quantity">Quantity</label>
                            <InputNumber v-model="saleNew.quantity" mode="decimal" :minFractionDigits="2" required="true" autofocus :class="{'p-invalid': submitted && !saleNew.quantity}" />
                            <small class="p-invalid" v-if="submitted && !saleNew.quantity">Quantity is required.</small>
                        </div>
                        <div class="field col col">
                            <label for="status">Active</label>
                            <br />
                            <InputSwitch v-model="saleNew.isEnabled" required="true" :class="{'p-invalid': submitted && !saleNew.isEnabled}" />
                            <small class="p-invalid" v-if="submitted && !saleNew.isEnabled">Active is required.</small>
                        </div>
                        <div class="field col col">
                            <br />
                            <Button label="Add Product" class="p-button-danger" v-if="customer.customerId" @click="saveNew" />
                        </div>
                    </div>

                    <div v-if="sale.saleProducts.length">
                        <DataTable ref="dt1" :value="sale.saleProducts" c dataKey="saleProductsId" :paginator="true" :rows="10" paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,25]" currentPageReportTemplate="Showing {first} to {last} of {totalRecords} items" responsiveLayout="scroll">
                            <Column field="saleProductsId" header="Id" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                                <template #body="slotProps">
                                    <span class="p-column-title">Id</span>
                                    {{slotProps.data.saleProductsId}}
                                </template>
                            </Column>

                            <Column field="name" header="Product" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                                <template #body="slotProps">
                                    <span class="p-column-title">Product</span>
                                    {{slotProps.data.product.name}}
                                </template>
                            </Column>

                            <Column field="unitPrice" header="Unit Price" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                                <template #body="slotProps">
                                    <span class="p-column-title">Unit Price</span>
                                    {{slotProps.data.unitPrice}}
                                </template>
                            </Column>

                            <Column field="quantity" header="Quantity" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                                <template #body="slotProps">
                                    <span class="p-column-title">Quantity</span>
                                    {{slotProps.data.quantity}}
                                </template>
                            </Column>

                            <Column field="isEnabled" header="Active" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                                <template #body="slotProps">
                                    <span class="p-column-title">Active</span>
                                    <InputSwitch v-model="slotProps.data.isEnabled" disabled />
                                </template>
                            </Column>
                            <Column headerStyle="min-width:10rem;">
                                <template #body="slotProps">
                                    <Button icon="pi pi-trash" class=" p-button-warning mt-2" v-show="slotProps.data.isEnabled != false" @click="deleteItemProduct(slotProps.index, slotProps.data)" />
                                </template>
                            </Column>
                        </DataTable>
                    </div>

                    <template #footer>
                        <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="hideDialog" />
                        <Button label="Save" icon="pi pi-check" class="p-button-text" @click="save" />
                    </template>
                </Dialog>

				<Dialog v-model:visible="deleteSaleDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="sale">Are you sure you want to delete <b>{{sale.saleId}}</b>?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteSaleDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteSale" />
					</template>
				</Dialog>
			</div>
		</div>
	</div>

</template>

<script>
    import { HTTP } from '@/service/http/http-services';
    import { FilterMatchMode } from 'primevue/api';

    export default {
        data() {
            return {
                saleDialog: false,
                deleteSaleDialog: false,
                deleteSalesDialog: false,
                selectedSales: null,
                submitted: false,
                sale: {
                    saleProducts: [],
                },
                saleNew: {},
                filters: {},
                items: [],
                products: [],
                product: {},
                customers: [],
                customer: {},
                paginationAndFilter: {
                    pageIndex: 1,
                    pageSize: 10,
                    pageTotals: 1,
                }
            }
        },
        computed: {
            total: function () {
                return this.calculateTotal();
            }
        },
        watch: {
            
        },
        created() {
            this.initFilters();
        },
        mounted() {
            this.getSales(this.paginationAndFilter.pageIndex);
            this.getCustomers();
            this.getProducts();
        },
        methods: {
            calculateTotal() {
                let total = 0;
                this.sale.saleProducts.forEach(item => {
                    total += item.unitPrice * item.quantity;
                });
                return  this.formatCurrency(total);
            },
            formatCurrency(value) {
                if (value)
                    return value.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                return;
            },
            getCustomers() {
                return HTTP.post('customer/customer-list', { pageIndex: 1, pageSize: 9999999 })
                    .then(response => {
                        this.customers = response.data.customers || [];
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            getProducts() {
                return HTTP.post('product/product-list', { pageIndex: 1, pageSize: 9999999 })
                    .then(response => {
                        this.products = response.data.products || [];
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            getSales(pageIndex) {
                this.paginationAndFilter.pageIndex = pageIndex;
                return HTTP.post('sale/sale-list', this.paginationAndFilter)
                    .then(response => {
                        this.items = response.data.sales || [];
                        this.paginationAndFilter.pageTotals = response.data.totalRow || 0;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            openNew() {
                this.total = 0;
                this.sale = {};
                this.sale.saleProducts = [];
                this.sale.saleId = 0;
                this.submitted = false;
                this.saleDialog = true;
            },
            hideDialog() {
                this.saleDialog = false;
                this.submitted = false;
            },
            save() {
                this.submitted = true;
                if (this.customer.customerId == undefined) {
                    this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: "Select One Customer", life: 3000 });
                    return false;
                }
                this.sale.customerId = this.customer.customerId;
                return HTTP.post('sale/sale-process', this.sale)
                    .then(response => {
                        console.log(response);
                        this.sale = {};
                        this.sale.saleProducts = [];
                        this.product = {};
                        this.getSales(this.paginationAndFilter.pageIndex);
                        this.saleDialog = false;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            edit(item) {
                this.saleDialog = true;
                return HTTP.get('sale/sale-detail/' + item.saleId)
                    .then(response => {
                        this.sale = response.data || {};
                        this.sale.saleId = item.saleId;
                        this.customer = this.sale.customer;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });

            },
            deleteItem(item) {
                this.sale = item;
                this.deleteSaleDialog = true;
            },
            deleteItemProduct(index, item) {
                if (item.saleId > 0) {
                    this.sale = item;
                    this.deleteSale();
                } else {
                    this.sale.saleProducts.splice(index, 1)
                }
            },
            deleteSale() {
                this.deleteSaleDialog = false;
                return HTTP.get('sale/sale-delete/' + this.sale.saleId)
                    .then(response => {
                        console.log(response);
                        this.edit(this.sale);
                        this.sale = {};
                        this.$toast.add({ severity: 'success', summary: 'Successful', detail: 'Sale Deleted', life: 3000 });
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });

            },
            initFilters() {
                this.filters = {
                    'global': { value: null, matchMode: FilterMatchMode.CONTAINS },
                }
            },
            exportCSV() {
                this.$refs.dt.exportCSV();
            },
            reset() {
                this.product = {};
                this.saleNew = {};
            },
            saveNew() {
                if (this.product.productId == undefined) {
                    this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: "Select One Product", life: 3000 });
                    return false;
                }

                if (this.product.unitPrice == undefined) {
                    this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: "Enter unit price", life: 3000 });
                    return false;
                }

                if (this.saleNew.quantity == undefined) {
                    this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: "Enter Quantity", life: 3000 });
                    return false;
                }

                let item = {
                    saleProductsId: 0,
                    productId: this.product.productId,
                    product: { name: this.product.name },
                    unitPrice: this.product.unitPrice,
                    quantity: this.saleNew.quantity,
                    isEnabled: true
                }
                if (this.sale.saleProducts == undefined)
                    this.sale.saleProducts = [];

                this.sale.saleProducts.push(item);
              
                this.reset();
            }
        }
    }
</script>

<style scoped lang="scss">
    @import '../assets/demo/badges.scss';
    label {
        font-weight: bold
    }
</style>

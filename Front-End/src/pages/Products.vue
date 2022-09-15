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
						<Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($product)" />
					</template>
				</Toolbar>

                <DataTable ref="dt" :value="items" v-model:selection="selectedProducts" dataKey="productId" :paginator="true" :rows="10" :filters="filters" paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,25]" currentPageReportTemplate="Showing {first} to {last} of {totalRecords} items" responsiveLayout="scroll">
                    <template #header>
                        <div class="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
                            <h5 class="m-0">Products</h5>
                            <span class="block mt-2 md:mt-0 p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['global'].value" placeholder="Search..." />
                            </span>
                        </div>
                    </template>

                    <Column field="productId" header="Id" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Id</span>
                            {{slotProps.data.productId}}
                        </template>
                    </Column>

                    <Column field="name" header="Name" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Name</span>
                            {{slotProps.data.name}}
                        </template>
                    </Column>

                    <Column field="inventory" header="Inventory" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Inventory</span>
                            {{slotProps.data.inventory}}
                        </template>
                    </Column>

                    <Column field="unitPrice" header="Unit Price" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Unit Price</span>
                            {{slotProps.data.unitPrice}}
                        </template>
                    </Column>

                    <Column field="active" header="Active" :sortable="true" headerStyle="width:14%; min-width:10rem;">
                        <template #body="slotProps">
                            <span class="p-column-title">Active</span>
                            <InputSwitch v-model="slotProps.data.isActive" disabled />
                        </template>
                    </Column>
                    <Column headerStyle="min-width:10rem;">
                        <template #body="slotProps">
                            <Button icon="pi pi-pencil" class="p-button-success mr-2" @click="edit(slotProps.data)" />
                            <Button icon="pi pi-trash" class=" p-button-warning mt-2" v-show="slotProps.data.isActive != false" @click="deleteItem(slotProps.data)" />
                        </template>
                    </Column>
                </DataTable>

				<Dialog v-model:visible="productDialog" :style="{width: '900px'}" header="Product Details" :modal="true" class="p-fluid">
					<div class="field">
						<label for="name">Name</label>
						<InputText id="name" v-model="product.name" required="true" autofocus :class="{'p-invalid': submitted && !product.name}" />
						<small class="p-invalid" v-if="submitted && !product.name">Name is required.</small>
					</div>
					<div class="field">
						<label for="unitPrice">Unit Price</label>
						<InputText id="unitPrice" v-model="product.unitPrice" required="true" autofocus :class="{'p-invalid': submitted && !product.unitPrice}" />
                        <small class="p-invalid" v-if="submitted && !product.unitPrice">Unit Price is required.</small>
					</div>
                    <div class="field">
                        <label for="inventory">Inventory</label>
                        <InputNumber v-model="product.inventory" mode="decimal" :minFractionDigits="2" required="true" autofocus :class="{'p-invalid': submitted && !product.inventory}" />
                        <small class="p-invalid" v-if="submitted && !product.inventory">Inventory is required.</small>
                    </div>
					<div class="field">
						<label for="status">Active</label>
						<br />
						<InputSwitch v-model="product.isActive" required="true" :class="{'p-invalid': submitted && !product.isActive}" />
						<small class="p-invalid" v-if="submitted && !product.isActive">Active is required.</small>
					</div>
					<template #footer>
						<Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="hideDialog" />
						<Button label="Save" icon="pi pi-check" class="p-button-text" @click="save" />
					</template>
				</Dialog>

				<Dialog v-model:visible="deleteProductDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="product">Are you sure you want to delete <b>{{product.name}}</b>?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteProductDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteProduct" />
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
                productDialog: false,
                deleteProductDialog: false,
				deleteProductsDialog: false,
                selectedProducts: null,
                submitted: false,
                product: {},
                filters: {},
                items: [],
                paginationAndFilter: {
                    pageIndex: 1,
                    pageSize: 10,
                    pageTotals: 1,
                }
            }
        },
        computed: {

        },
        created() {
            this.initFilters();
        },
        mounted() {
            this.getProducts(this.paginationAndFilter.pageIndex);
        },
        methods: {
            getProducts(pageIndex) {
                this.paginationAndFilter.pageIndex = pageIndex;
                return HTTP.post('product/product-list', this.paginationAndFilter)
                    .then(response => {
                        this.items = response.data.products || [];
                        this.paginationAndFilter.pageTotals = response.data.totalRow || 0;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            formatCurrency(value) {
                if (value)
                    return value.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                return;
            },
            openNew() {
                this.product = {};
                this.product.productId = 0;
                this.submitted = false;
                this.productDialog = true;
            },
            hideDialog() {
                this.productDialog = false;
                this.submitted = false;
            },
            save() {
                this.submitted = true;
                debugger;
                if (this.product.name == undefined)
                    return false;
                return HTTP.post('product/product-process', this.product)
                    .then(response => {
                        console.log(response);
                        this.product = {};
                        this.getProducts(this.paginationAndFilter.pageIndex);
                        this.productDialog = false;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            edit(item) {
                this.productDialog = true;
                return HTTP.get('product/product-detail/' + item.productId)
					.then(response => {
                        this.product = response.data || {};
                        this.product.productId = item.productId;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail:  e.response.data.errors[0].errorMessage, life: 3000 });
                    });

            },
            deleteItem(item) {
                this.product = item;
                this.deleteProductDialog = true;
            },
            deleteProduct() {
                this.deleteProductDialog = false;
                return HTTP.get('product/product-delete/' + this.product.productId)
					.then(response => {
						console.log(response);
                        this.product = {};
                        this.getProducts(this.paginationAndFilter.pageIndex);
                        this.$toast.add({ severity: 'success', summary: 'Successful', detail: 'Product Deleted', life: 3000 });
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
        }
    }
</script>

<style scoped lang="scss">
    @import '../assets/demo/badges.scss';
    label {
        font-weight: bold
    }
</style>

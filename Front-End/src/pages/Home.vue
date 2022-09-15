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
						<Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($customer)" />
					</template>
				</Toolbar>

				<DataTable ref="dt" :value="items" v-model:selection="selectedCustomers" dataKey="customerId" :paginator="true" :rows="10" :filters="filters" paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,25]" currentPageReportTemplate="Showing {first} to {last} of {totalRecords} items" responsiveLayout="scroll">
					<template #header>
						<div class="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
							<h5 class="m-0">Customers</h5>
							<span class="block mt-2 md:mt-0 p-input-icon-left">
								<i class="pi pi-search" />
								<InputText v-model="filters['global'].value" placeholder="Search..." />
							</span>
						</div>
					</template>

					<Column field="customerId" header="Id" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Id</span>
							{{slotProps.data.customerId}}
						</template>
					</Column>

					<Column field="firstName" header="FirstName" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">FirstName</span>
							{{slotProps.data.firstName}}
						</template>
					</Column>
					<Column field="lastName" header="LastName" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">LastName</span>
							{{slotProps.data.lastName}}
						</template>
					</Column>
					<Column field="identification" header="Identification" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Identification</span>
							{{slotProps.data.identification}}
						</template>
					</Column>
					<Column field="email" header="Email" :sortable="true" headerStyle="width:14%; min-width:8rem;">
						<template #body="slotProps">
							<span class="p-column-title">Email</span>
							{{slotProps.data.email}}
						</template>
					</Column>
					<Column field="phone" header="Phone" :sortable="true" headerStyle="width:14%; min-width:8rem;">
						<template #body="slotProps">
							<span class="p-column-title">Phone</span>
							{{slotProps.data.phone}}
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

				<Dialog v-model:visible="customerDialog" :style="{width: '900px'}" header="Customer Details" :modal="true" class="p-fluid">
					<div class="field">
						<label for="name">Identification</label>
						<InputText id="identification" v-model="customer.identification" required="true" autofocus :class="{'p-invalid': submitted && !customer.identification}" />
						<small class="p-invalid" v-if="submitted && !customer.identification">Identification is required.</small>
					</div>
					<div class="field">
						<label for="firstName">First Name</label>
						<InputText id="firstName" v-model="customer.firstName" required="true" autofocus :class="{'p-invalid': submitted && !customer.firstName}" />
						<small class="p-invalid" v-if="submitted && !customer.firstName">First Name is required.</small>
					</div>
					<div class="field">
						<label for="lastName">Last Name</label>
						<InputText id="lastName" v-model="customer.lastName" required="true" autofocus :class="{'p-invalid': submitted && !customer.lastName}" />
						<small class="p-invalid" v-if="submitted && !customer.lastName">Last Name is required.</small>
					</div>
					<div class="field">
						<label for="phone">Phone</label>
						<InputText id="phone" v-model="customer.phone" required="true" autofocus :class="{'p-invalid': submitted && !customer.phone}" />
						<small class="p-invalid" v-if="submitted && !customer.lastName">Phone is required.</small>
					</div>
					<div class="field">
						<label for="email">Email</label>
						<InputText id="email" v-model="customer.email" required="true" autofocus :class="{'p-invalid': submitted && !customer.email}" />
						<small class="p-invalid" v-if="submitted && !customer.email">Email is required.</small>
					</div>
					<div class="field">
						<label for="status">Active</label>
						<br />
						<InputSwitch v-model="customer.isActive" required="true" :class="{'p-invalid': submitted && !customer.isActive}" />
						<small class="p-invalid" v-if="submitted && !customer.isActive">Active is required.</small>
					</div>
					<template #footer>
						<Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="hideDialog" />
						<Button label="Save" icon="pi pi-check" class="p-button-text" @click="save" />
					</template>
				</Dialog>

				<Dialog v-model:visible="deleteCustomerDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="customer">Are you sure you want to delete <b>{{customer.fullName}}</b>?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteCustomerDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteCustomer" />
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
                customerDialog: false,
                deleteCustomerDialog: false,
				deleteCustomersDialog: false,
                selectedCustomers: null,
                submitted: false,
                customer: {},
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
            this.getCustomers(this.paginationAndFilter.pageIndex);
        },
        methods: {
            getCustomers(pageIndex) {
                this.paginationAndFilter.pageIndex = pageIndex;
                return HTTP.post('customer/customer-list', this.paginationAndFilter)
                    .then(response => {
                        this.items = response.data.customers || [];
                        this.paginationAndFilter.pageTotals = response.data.totalRow || 0;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            openNew() {
                this.customer = {};
                this.customer.customerId = 0;
                this.submitted = false;
                this.customerDialog = true;
            },
            hideDialog() {
                this.customerDialog = false;
                this.submitted = false;
            },
            save() {
				this.submitted = true;
                if (this.product.identification == undefined)
                    return false;
                return HTTP.post('customer/customer-process', this.customer)
                    .then(response => {
                        console.log(response);
                        this.customer = {};
                        this.getCustomers(this.paginationAndFilter.pageIndex);
                        this.customerDialog = false;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            edit(item) {
                this.customerDialog = true;
                return HTTP.get('customer/customer-detail/' + item.customerId)
					.then(response => {
                        this.customer = response.data || {};
                        this.customer.customerId = item.customerId;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail:  e.response.data.errors[0].errorMessage, life: 3000 });
                    });

            },
            deleteItem(item) {
                this.customer = item;
                this.deleteCustomerDialog = true;
            },
            deleteCustomer() {
                this.deleteCustomerDialog = false;
                return HTTP.get('customer/customer-delete/' + this.customer.customerId)
					.then(response => {
						console.log(response);
                        this.customer = {};
                        this.getCustomers(this.paginationAndFilter.pageIndex);
                        this.$toast.add({ severity: 'success', summary: 'Successful', detail: 'Customer Deleted', life: 3000 });
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

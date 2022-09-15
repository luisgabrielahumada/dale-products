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
						<Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($event)" />
					</template>
				</Toolbar>

				<DataTable ref="dt" :value="items" v-model:selection="selectedProducts" dataKey="eventId" :paginator="true" :rows="10" :filters="filters"
							paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,25]"
							currentPageReportTemplate="Showing {first} to {last} of {totalRecords} events" responsiveLayout="scroll">
					<template #header>
						<div class="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
							<h5 class="m-0">Event List</h5>
							<span class="block mt-2 md:mt-0 p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['global'].value" placeholder="Search..." />
                            </span>
						</div>
					</template>

					<Column field="eventId" header="EventId" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">EventId</span>
							{{slotProps.data.eventId}}
						</template>
					</Column>

					<Column field="name" header="Name" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Name</span>
							{{slotProps.data.code}}/ {{slotProps.data.name}}
						</template>
					</Column>
					<Column header="Image" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Image</span>
							<img :src="slotProps.data.icon" :alt="slotProps.data.icon" class="shadow-2" width="100" />
						</template>
					</Column>
					<Column field="country" header="Country" :sortable="true" headerStyle="width:14%; min-width:8rem;">
						<template #body="slotProps">
							<span class="p-column-title">Country</span>
							{{slotProps.data.country}}
						</template>
					</Column>
					<Column field="active" header="Active" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Active</span>
							<InputSwitch v-model="slotProps.data.active"  disabled />
						</template>
					</Column>
					<Column field="startDate" header="Date" :sortable="true" headerStyle="width:14%; min-width:8rem;">
						<template #body="slotProps">
							<span class="p-column-title">Date</span>
							{{slotProps.data.schedule.dates1}}, {{slotProps.data.schedule.dates2}}
						</template>
					</Column>
					<Column headerStyle="min-width:10rem;">
						<template #body="slotProps">
							<Button icon="pi pi-pencil" class="p-button-success mr-2" @click="editEvent(slotProps.data)" />
							<Button icon="pi pi-trash" class=" p-button-warning mt-2" @click="confirmDeleteEvent(slotProps.data)" />
						</template>
					</Column>
				</DataTable>

				<Dialog v-model:visible="eventDialog" :style="{width: '900px'}" header="Event Details" :modal="true" class="p-fluid">
					<div class="field">
						<label for="name">Code</label>
						<InputText id="code" v-model="event.code" required="true" autofocus :class="{'p-invalid': submitted && !event.code}" />
						<small class="p-invalid" v-if="submitted && !event.code">Code is required.</small>
					</div>
					<div class="field">
						<label for="country">Country</label>
						<InputText id="country" v-model="event.country" required="true" autofocus :class="{'p-invalid': submitted && !event.country}" />
						<small class="p-invalid" v-if="submitted && !event.country">Country is required.</small>
					</div>
					<div class="form grid">
						<div class="field col">
							<label for="startDate">Start Date</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd" :showButtonBar="true" v-model="event.startDate" required="true" :class="{'p-invalid': submitted && !event.startDate}" :showTime="true" hourFormat="12"></Calendar>
							<small class="p-invalid" v-if="submitted && !event.startDate">Start Date is required.</small>
						</div>
						<div class="field col">
							<label for="endDate">End Date</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd" :showButtonBar="true" v-model="event.endDate" required="true" :class="{'p-invalid': submitted && !event.startDate}" :showTime="true" hourFormat="12"></Calendar>
							<small class="p-invalid" v-if="submitted && !event.endDate">End Date is required.</small>
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="icon">Icon</label>
							<InputText id="icon" v-model="event.icon" required="true" autofocus :class="{'p-invalid': submitted && !event.icon}" />
							<small class="p-invalid" v-if="submitted && !event.icon">Icon is required.</small>
						</div>
						<div class="field col">
							<img :src="event.icon" style="width:20%" />
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="image">Image</label>
							<InputText id="image" v-model="event.image" autofocus />
						</div>
						<div class="field col">
							<img :src="event.image" style="width:20%" />
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="image2">Other Image</label>
							<InputText id="image2" v-model="event.image2" autofocus />
						</div>
						<div class="field col">
							<img :src="event.image2" style="width:20%" />
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="background">Background</label>
							<InputText id="background" v-model="event.background" autofocus />
						</div>
						<div class="field col">
							<img :src="event.background" style="width:20%" />
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="video">Video</label>
							<InputText id="video" v-model="event.video" autofocus />
						</div>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="status">Active</label>
							<br />
							<InputSwitch v-model="event.active" required="true" :class="{'p-invalid': submitted && !event.active}" />
							<small class="p-invalid" v-if="submitted && !event.active">Active is required.</small>
						</div>
						<div class="field col">
							<label for="physical">Physical</label>
							<br />
							<InputSwitch v-model="event.physical" required="true" :class="{'p-invalid': submitted && !event.physical}" />
							<small class="p-invalid" v-if="submitted && !event.physical">Physical is required.</small>
						</div>
						<div class="field col">
							<label for="virtual">Virtual</label>
							<br />
							<InputSwitch v-model="event.virtual" required="true" :class="{'p-invalid': submitted && !event.virtual}" />
							<small class="p-invalid" v-if="submitted && !event.virtual">Virtual is required.</small>
						</div>
					</div>
					<div class="field">
						<label for="location">Location</label>
						<InputText id="location" v-model="event.location" required="true" autofocus :class="{'p-invalid': submitted && !event.location}" />
						<small class="p-invalid" v-if="submitted && !event.location">Location is required.</small>
					</div>
					<div class="field">
						<label for="link">Link</label>
						<InputText id="link" v-model="event.link" required="true" autofocus :class="{'p-invalid': submitted && !event.link}" />
						<small class="p-invalid" v-if="submitted && !event.link">Link is required.</small>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="streamStart">Stream Start</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.streamStart" required="true" :class="{'p-invalid': submitted && !event.streamStart}" :showTime="true" hourFormat="12"></Calendar>
							<small class="p-invalid" v-if="submitted && !event.streamStart">Stream Start is required.</small>
						</div>
						<div class="field col">
							<label for="streamEnd">Stream End</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.streamEnd" required="true" :class="{'p-invalid': submitted && !event.streamEnd}" :showTime="true" hourFormat="12"></Calendar>
							<small class="p-invalid" v-if="submitted && !event.streamEnd">Stream End is required.</small>
						</div>
						<div class="field col">
							<label for="streamCutOff">Stream Cut Off</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.streamCutOff"   :showTime="true" hourFormat="12"></Calendar>
						</div>
					</div>
					<div class="field">
						<label for="ticketLink">Ticket Link</label>
						<InputText id="ticketLink" v-model="event.ticketLink" required="true" autofocus :class="{'p-invalid': submitted && !event.ticketLink}" />
						<small class="p-invalid" v-if="submitted && !event.ticketLink">Ticket Link is required.</small>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="ticketStart">Ticket Start</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.ticketStart"  :showTime="true" hourFormat="12"></Calendar>
						</div>
						<div class="field col">
							<label for="ticketEnd">Ticket End</label>
							<Calendar :showIcon="true"  dateFormat="yy-mm-dd" :showButtonBar="true" v-model="event.ticketEnd"  :showTime="true" hourFormat="12"></Calendar>
						</div>
						<div class="field col">
							<label for="ticketCutOff">Ticket Cut Off</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.ticketCutOff"  :showTime="true" hourFormat="12"></Calendar>
						</div>
					</div>
					<div class="field">
						<label for="ticketPeopleQuery">Ticket People Query</label>
						<InputText id="ticketPeopleQuery" v-model="event.ticketPeopleQuery" required="true" autofocus :class="{'p-invalid': submitted && !event.ticketPeopleQuery}" />
						<small class="p-invalid" v-if="submitted && !event.ticketPeopleQuery">Ticket People Query is required.</small>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="ticketPeopleMax">Ticket People Max</label>
							<InputNumber :showIcon="true" :showButtonBar="true" v-model="event.ticketPeopleMax" required="true" :class="{'p-invalid': submitted && !event.ticketPeopleMax}"></InputNumber>
							<small class="p-invalid" v-if="submitted && !event.ticketPeopleMax">Ticket Start is required.</small>
						</div>
						<div class="field col">
							<label for="ticketVirtualPeopleMax">Ticket Virtual People Max</label>
							<InputNumber :showIcon="true" :showButtonBar="true" v-model="event.ticketVirtualPeopleMax" required="true" :class="{'p-invalid': submitted && !event.ticketVirtualPeopleMax}"></InputNumber>
							<small class="p-invalid" v-if="submitted && !event.ticketVirtualPeopleMax">Ticket Virtual People Max is required.</small>
						</div>
						<div class="field col">
							<label for="ticketVirtualQuery">Ticket Virtual Query</label>
							<InputNumber :showIcon="true" :showButtonBar="true" v-model="event.ticketVirtualQuery" required="true" :class="{'p-invalid': submitted && !event.ticketVirtualQuery}"></InputNumber>
							<small class="p-invalid" v-if="submitted && !event.ticketVirtualQuery">Ticket Virtual Query is required.</small>
						</div>
					</div>

					<div class="field">
						<label for="shopLink">Shop Link</label>
						<InputText id="shopLink" v-model="event.shopLink" required="true" autofocus :class="{'p-invalid': submitted && !event.shopLink}" />
						<small class="p-invalid" v-if="submitted && !event.shopLink">Shop Link is required.</small>
						<span v-if="event.shopLink"><a class="mb-2" :href="event.shopLink" target="_blank"><span class="pi pi-shopping-cart"></span> Purchase</a></span>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="shopStart">Shop Start</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.shopStart"  :showTime="true" hourFormat="12"></Calendar>
						</div>
						<div class="field col">
							<label for="shopEnd">Shop End</label>
							<Calendar :showIcon="true" dateFormat="yy-mm-dd"  :showButtonBar="true" v-model="event.shopEnd"  :showTime="true" hourFormat="12"></Calendar>
						</div>
						<div class="field col">
							<label for="shopCutOff">Shop CutOff</label>
							<Calendar :showIcon="true"  dateFormat="yy-mm-dd" :showButtonBar="true" v-model="event.shopCutOff" :showTime="true" hourFormat="12"></Calendar>
						</div>
					</div>


					<div class="field">
						<label for="messageEnglish">Message English</label>
						<Textarea id="messageEnglish" v-model="event.messageEnglish" rows="3" cols="20"   :class="{'p-invalid': submitted && !event.messageEnglish}" />
						<small class="p-invalid" v-if="submitted && !event.messageEnglish">Message English is required.</small>
					</div>
					<div class="field">
						<label for="messageSpanish">Message Spanish</label>
						<Textarea id="messageSpanish" v-model="event.messageSpanish" rows="3" cols="20"  :class="{'p-invalid': submitted && !event.messageFrench}" />
						<small class="p-invalid" v-if="submitted && !event.messageFrench">Message Spanish is required.</small>
					</div>
					<div class="field">
						<label for="messageFrench">Message French</label>
						<Textarea id="messageFrench" v-model="event.messageFrench" rows="3" cols="20"  :class="{'p-invalid': submitted && !event.messageFrench}" />
						<small class="p-invalid" v-if="submitted && !event.messageFrench">Message French is required.</small>
					</div>
					<div class="field">
						<label for="messageFrench">Message Portugues</label>
						<Textarea id="messagePortugues" v-model="event.messagePortugues" rows="3" cols="20"  :class="{'p-invalid': submitted && !event.messagePortugues}" />
						<small class="p-invalid" v-if="submitted && !event.messagePortugues">Message Portugues is required.</small>
					</div>
					<div class="field">
						<label for="messageFrench">Message Italian</label>
						<Textarea id="messageItalian" v-model="event.messageItalian" rows="3" cols="20"  :class="{'p-invalid': submitted && !event.messageItalian}" />
						<small class="p-invalid" v-if="submitted && !event.messageItalian">Message Italian is required.</small>
					</div>
					<div class="formgrid grid">
						<div class="field col">
							<label for="orderID">Order By</label>
							<InputNumber :showIcon="true" :showButtonBar="true" v-model="event.orderID" required="true" :class="{'p-invalid': submitted && !event.orderID}"></InputNumber>
							<small class="p-invalid" v-if="submitted && !event.orderID">Order By is required.</small>
						</div>
					</div>
					<template #footer>
						<Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="hideDialog" />
						<Button label="Save" icon="pi pi-check" class="p-button-text" @click="saveEvent" />
					</template>
				</Dialog>

				<Dialog v-model:visible="deleteEventDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="event">Are you sure you want to delete <b>{{event.name}}</b>?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteEventDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteEvent" />
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
                eventDialog: false,
                deleteEventDialog: false,
                deleteEventsDialog: false,
                event: {},
                selectedProducts: null,
                filters: {},
                submitted: false,
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
            this.goEvents(this.paginationAndFilter.pageIndex);
        },
        methods: {
            goEvents(pageIndex) {
                this.paginationAndFilter.pageIndex = pageIndex;
                return HTTP.post('events/events-info', this.paginationAndFilter)
                    .then(response => {
                        this.items = response.data.events || [];
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
                this.event = {};
                this.event.eventId = 0;
                this.submitted = false;
                this.eventDialog = true;
            },
            hideDialog() {
                this.eventDialog = false;
                this.submitted = false;
            },
            saveEvent() {
                this.submitted = true;
                return HTTP.post('events/event-process', this.event)
                    .then(response => {
                        console.log(response);
						this.event = {};
                        this.goEvents(this.paginationAndFilter.pageIndex);
                        this.eventDialog = false;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            editEvent(event) {
                this.eventDialog = true;
                return HTTP.get('events/event-detail/' + event.eventId)
					.then(response => {
                        this.event = response.data || {};
                        this.event.eventId = event.eventId;
                    })
                    .catch(e => {
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail:  e.response.data.errors[0].errorMessage, life: 3000 });
                    });

            },
            confirmDeleteEvent(event) {
                this.event = event;
                this.deleteEventDialog = true;
            },
            deleteEvent() {
                this.deleteEventDialog = false;
                return HTTP.get('events/event-delete/' + this.event.eventId)
                    .then(response => {
                        console.log(response);
                        this.event = {};
                        this.goEvents(this.paginationAndFilter.pageIndex);
                        this.$toast.add({ severity: 'success', summary: 'Successful', detail: 'Event Deleted', life: 3000 });
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

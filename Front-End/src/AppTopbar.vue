<template>
	<div class="layout-topbar">
		<router-link to="/" class="layout-topbar-logo">
			<img alt="Logo" src="https://www.dale.com.co/assets/images/logo_dale_oscuro.svg" />
		</router-link>
		<button class="p-link layout-menu-button layout-topbar-button" @click="onMenuToggle">
			<i class="pi pi-bars"></i>
		</button>

		<button class="p-link layout-topbar-menu-button layout-topbar-button"
			v-styleclass="{ selector: '@next', enterClass: 'hidden', enterActiveClass: 'scalein', 
			leaveToClass: 'hidden', leaveActiveClass: 'fadeout', hideOnOutsideClick: true}">
			<i class="pi pi-ellipsis-v"></i>
		</button>
		<ul class="layout-topbar-menu hidden lg:flex origin-top">
			<!--<li>
				<button class="p-link layout-topbar-button">
					<i class="pi pi-calendar"></i>
					<span>Events</span>
				</button>
			</li>
			<li>
				<button class="p-link layout-topbar-button">
					<i class="pi pi-cog"></i>
					<span>Settings</span>
				</button>
			</li>-->
			<li>
				<button class="p-link layout-topbar-button" @click="logout()">
					<i class="pi pi-user"></i>
					<span>Profile</span>
				</button>
			</li>
		</ul>
	</div>
</template>

<script>
export default {
    methods: {
        onMenuToggle(event) {
            this.$emit('menu-toggle', event);
        },
		onTopbarMenuToggle(event) {
            this.$emit('topbar-menu-toggle', event);
		},
        logout() {
            localStorage.removeItem("auth_userId");
            localStorage.removeItem("auth_login");
            localStorage.removeItem("auth_email");
            localStorage.removeItem("auth_sessionId");
            localStorage.removeItem("auth_name");
            localStorage.removeItem("access_token");
            localStorage.removeItem("loggedIn", false);
            window.localStorage.setItem("authenticated", false);
            this.$router.push("/");
        },
    },
	computed: {
		darkTheme() {
			return this.$appState.darkTheme;
		}
	}
}
</script>
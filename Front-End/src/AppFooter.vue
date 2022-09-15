<template>
	<div class="layout-footer">
		<img alt="Logo" src="https://cdn.immunotec.com/docs/images/Immunotec-logo.svg" height="20" class="mr-2" />
		by
		<span class="font-medium ml-2">Immunotec</span>
	</div>
</template>

<script>
	export default {
		name: "AppFooter",
		methods: {
			footerImage() {
				return this.$appState.darkTheme ? 'images/logo-white.svg' : 'images/logo-dark.svg';
			}
		},
		computed: {
			darkTheme() {
				return this.$appState.darkTheme;
			}
		}
	}
</script>
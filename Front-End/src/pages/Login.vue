<template>
    <div class="surface-0 flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="grid justify-content-center p-2 lg:p-0" style="min-width:80%">
            <Toast />
            <div class="col-12 xl:col-6" style="border-radius:56px; padding:0.3rem;">
                <div class="h-full w-full m-0 py-7 px-4" style="border-radius:53px;">
                    <div class="col-12 mt-5 xl:mt-0 text-center">
                        <img src="https://www.dale.com.co/assets/images/logo_dale_oscuro.svg" alt="Dale!" height="50" class="mr-0 lg:mr-2">
                    </div>
                    <div class="text-center mb-5">
                        <h1>Welcome</h1>
                        <span class="text-600 font-medium">Sign in to continue</span>
                    </div>
                    <form @submit.prevent="doLogin">
                        <div class="w-full md:w-10 mx-auto">
                            <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                            <InputText id="email1" v-model="model.email" type="text" class="w-full mb-3" placeholder="Email" style="padding:1rem;" />

                            <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                            <Password id="password1" v-model="model.password" placeholder="Password" :toggleMask="true" class="w-full mb-3" inputClass="w-full" inputStyle="padding:1rem"></Password>
                            <Button label="Sign In" type="submit" class="w-full p-3 text-xl"></Button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { HTTP } from '@/service/http/http-services';
    export default {
        data() {
            return {
                loading: false,
                model: {
                    email: '',
                    password: '',
                }
            }
        },
        computed: {

        },
        methods: {
            doLogin() {
                this.loading = true;
                return HTTP.post("Auth/Login", {
                    login: this.model.email,
                    password: this.model.password,
                })
                    .then((response) => {
                        this.loading = false;
                        localStorage.setItem("name", response.data.name);
                        localStorage.setItem("userId", response.data.userId);
                        localStorage.setItem("email", response.data.email);
                        localStorage.setItem("login", response.data.login);
                        localStorage.setItem("_Token", response.data._Token);
                        localStorage.setItem("sessionId", response.data.sessionId);
                        localStorage.setItem("loggedIn", true);
                        window.localStorage.setItem("authenticated", true);
                        if (this.$route.name !== "/home") {
                            this.$router.push("/home");
                        }
                    })
                    .catch((e) => {
                        this.loading = false;
                        console.log(e.response.data.errors[0].errorMessage);
                        this.$toast.add({ severity: 'error', summary: 'Warn Message', detail: e.response.data.errors[0].errorMessage, life: 3000 });
                    });
            },
            doRegister() { },
        },
    }
</script>

<style scoped>
    .pi-eye {
        transform: scale(1.6);
        margin-right: 1rem;
    }

    .pi-eye-slash {
        transform: scale(1.6);
        margin-right: 1rem;
    }
</style>
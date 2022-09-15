<template>
    <div class="grid">
        <div class="col-12 md:col-12 align-content-center justify-content-center text-center">
            <div class="space-5"></div>
            <Button label="ENG" @click="setLanguage('EN')" class="p-button-outlined p-button-danger mr-2 mb-2"></Button>
            <Button label="ESP" @click="setLanguage('ES')" class="p-button-outlined p-button-success mr-2 mb-2" ></Button>
            <Button label="FRE" @click="setLanguage('FR')" class="p-button-outlined p-button-warning mr-2 mb-2" ></Button>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
               
            };
        },
        methods: {
            setLanguage(lang) {
                localStorage.setItem("lang", lang);
            }
        },
        computed: {

        },
        mounted() {
        }
    }</script>
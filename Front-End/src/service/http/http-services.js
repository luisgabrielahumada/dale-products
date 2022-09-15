import axios from 'axios';
export const HTTP = axios.create({
    //baseURL: "https://immunotec-events-api-dev.azurewebsites.net/api/v1/",
     baseURL: "https://localhost:44362/api/v1/",
    headers: {
        "Cache-Control": "no-cache",
        "Access-Control-Allow-Origin": "*",
    }
});
HTTP.interceptors.request.use(function (config) {
    const access_token = localStorage.getItem('_Token');
    if (access_token) {
        config.headers.common = {
            "Authorization": `Bearer ${access_token}`,
            "Accept": "application/json, text/plain, */*",
            "Content-Type": "application/json"
        };
    }
    return config;
}, function (err) {
    return Promise.reject(err);
});
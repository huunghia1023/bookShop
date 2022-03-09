import axios from 'axios';

const AxiosClient = axios.create({
    baseURL: 'https://localhost:5000/',
    headers: {
        'Content-Type': 'application/json',
    }
});
export default AxiosClient;
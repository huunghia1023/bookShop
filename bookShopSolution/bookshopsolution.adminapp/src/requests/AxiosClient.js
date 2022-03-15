import axios from 'axios';
import { BaseAddress } from '../utils/constant';

const AxiosClient = axios.create({
    baseURL: BaseAddress,
    headers: {
        'Content-Type': 'application/json',
    }
});
export default AxiosClient;
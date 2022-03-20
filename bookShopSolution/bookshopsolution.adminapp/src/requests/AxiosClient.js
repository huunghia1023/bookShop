import axios from 'axios';
import { BaseAddress } from '../utils/Constant';

const AxiosClient = axios.create({
    baseURL: BaseAddress,
    headers: {
        'Content-Type': 'application/json',
    }
});
export default AxiosClient;
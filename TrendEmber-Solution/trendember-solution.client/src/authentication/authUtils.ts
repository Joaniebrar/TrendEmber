import { User } from './types';
import axios from 'axios';

const API_BASE_URL = '/api'; 

// Simulates fetching the currently logged-in user
export const getUser = async (): Promise<User | null> => {
    const response = await axios.get(`${API_BASE_URL}/auth/user`, {
        withCredentials: true,
    });
    return response.data;
};

// Simulates logging in
export const loginUser = async (email: string, password: string): Promise<string> => {
    const response = await axios.post(`${API_BASE_URL}/Authentication/login`, {
        email,
        password,
    });
    return response.data?.token;
};

// Simulates logging out
export const logoutUser = async (): Promise<void> => {
    await axios.post(
        `${API_BASE_URL}/Authentication/logout`,
        {}, // Empty body for logout request (if needed)
        {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('authToken')}`
            }
        }
    );
};

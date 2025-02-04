import { User } from './types';

const API_BASE_URL = '/api'; 

export const getUser = async (): Promise<User | null> => {
    const response = await fetch(`${API_BASE_URL}/auth/user`, {
        method: "GET"
    });
    return await response.json();
};

export const loginUser = async (email: string, password: string): Promise<string> => {
    const response = await fetch(`${API_BASE_URL}/Authentication/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            email,
            password,
        })
    });
    const data = await response.json();
    return data.token;
};

export const logoutUser = async (): Promise<void> => {
    await fetch(
        `${API_BASE_URL}/Authentication/logout`,
        {
            method: "POST",
            headers: {
                Authorization: `Bearer ${localStorage.getItem('authToken')}`,
                "Content-Type": "application/json",
            }
        }
    );
};

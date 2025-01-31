import {useState} from 'react';
import { useAuth } from '../../authentication/useAuth';
import logo from "../../assets/logo.png"
const Login = () => {
    const {login, error} = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading,setIsLoading] = useState(false);

    const handleLoginClick = async () => {
        setIsLoading(true);
        await login(email,password);
        setIsLoading(false);
    };

    return (
        <div className = "login-container">
            <img src={logo} alt="TrendEmber Logo" className="login-logo" />            
            {isLoading && <span className='loading-spinner'></span>}
            <form className="login-form" onSubmit={(e) => e.preventDefault()}>                
                <input type="text" placeholder="Email" value={email} onChange ={(e)=> setEmail(e.target.value)}/>
                <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                <button type="submit" onClick={(e) => { e.preventDefault(); handleLoginClick(); } }>login</button>                
                <div className="error-container">
                    {error && <span className ="error-message">{error}</span>}
                </div>
            </form>
        </div>
    )};

export default Login;
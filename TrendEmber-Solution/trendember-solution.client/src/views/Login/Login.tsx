import {useState} from 'react';
import { useAuth } from '../../authentication/useAuth';
import logo from "../../assets/logo.png"
import { useNavigate } from 'react-router-dom';

const Login = () => {
    const {login: authenticate, error} = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading,setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleLoginClick = async () => {
        setIsLoading(true);
        await authenticate(email,password)
            .then((result: boolean) =>{
                if (result){
                    navigate('/');
                }
            })
            .finally(()=>setIsLoading(false));        
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleLoginClick();
      };

    return (
        <div className = "login-container">
            <img src={logo} alt="TrendEmber Logo" className="login-logo" />            
            {isLoading && <span className='loading-spinner'></span>}
            <form className="login-form" onSubmit={handleSubmit}>                
                <input id="loginEmail" type="text" placeholder="Email" value={email} onChange ={(e)=> setEmail(e.target.value)}/>
                <input id="loginPassword"  type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                <button id="loginBtn" type="submit">login</button>                
                <div className="error-container">
                    {error && <span className ="error-message">{error}</span>}
                </div>
            </form>
        </div>
    )};

export default Login;
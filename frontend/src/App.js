// App.js
import React from 'react';
import Login from './components/Auth/Login';
import {Route, Routes, useNavigate} from 'react-router-dom';
import Layout from './Layout/Layout';
import Dashboard from './components/Other/Dashboard';
import EmployeeSettings from './components/Profile/For HR/EmployeeSettings';
import axios from 'axios';
import Account from './components/Profile/For user/Account';
import Security from "./components/Profile/For user/Security";
import NotExistPage from "./components/Other/NotExistPage";
import ProtectedRoute from "./components/small-components/ProtectedRoute";
import {useCookies} from "react-cookie";
import EmployeesList from "./components/Profile/For HR/EmployeeList";

const App = () => {
  axios.defaults.baseURL = 'https://localhost:7087';
  //axios.defaults.xsrfHeaderName = "X-CSRFTOKEN";
  //axios.defaults.xsrfCookieName = "csrftoken";
  const [cookies, , removeCookie] = useCookies();
  const [user, setUser] = React.useState(cookies.user);
  const navigate = useNavigate();

  const logoutHandler = () => {
    setUser(null);
    removeCookie('user');
    navigate('/login');
  }

  return (
    <div className='App'>
        <Routes>
          <Route index path="login" element={<Login />} />
          <Route element={
            <Layout user={user} logoutHandler={logoutHandler}>
              <ProtectedRoute isAllowed={!!user}/>
            </Layout> }>
            {/*=======For authorized users=========*/}
              <Route path="" element={<Dashboard />} />
              <Route path="account" element={<Account />} />
              <Route path="security" element={<Security />} />

              <Route path="employees" element={<EmployeesList />} />
              <Route path="profilesettings" element={<EmployeeSettings />} />
            {/*=======!For authorized users=========*/}
          </Route>
          <Route path="*" element={<NotExistPage />} />
        </Routes>
    </div>
  );
};

export default App;

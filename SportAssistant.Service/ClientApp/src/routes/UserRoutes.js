import LoginUser from "../components/userData/auth/LoginUser";
import RegisterUser from "../components/userData/auth/RegisterUser";
import ChangeUserPassword from "../components/userData/auth/ChangeUserPassword";
import UserCabinet from "../components/userData/UserCabinet";

const UserRoutes = [
  { path: '/login', element: <LoginUser /> },
  { path: '/register', element: <RegisterUser /> },
  { path: '/changePassword', element: <ChangeUserPassword /> },

  { path: '/userCabinet', element: <UserCabinet />}
];

export default UserRoutes;
import RegisterUser from "../components/userData/auth/RegisterUser";
import ChangeUserPassword from "../components/userData/auth/ChangeUserPassword";
import ResetUserPassword from "../components/userData/auth/ResetUserPassword";
import UserCabinet from "../components/userData/UserCabinet";

const UserRoutes = [
  { path: '/register', element: <RegisterUser /> },
  { path: '/changePassword', element: <ChangeUserPassword /> },
  { path: '/resetPassword', element: <ResetUserPassword /> },
  
  { path: '/userCabinet', element: <UserCabinet />}
];

export default UserRoutes;
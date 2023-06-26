
import OrgEdit from "../components/management/organization/OrgEdit";
import ManagerListView from "../components/management/organization/OrgEdit";


const ManagementRoutes = [
  { path: '/organization/edit/:id', element: <OrgEdit /> },
  { path: '/manager/list', element: <ManagerListView /> },
];

export default ManagementRoutes;
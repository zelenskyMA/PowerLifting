
import OrgEdit from "../components/management/organization/OrgEdit";
import ManagerListView from "../components/management/manager/ManagerListView";
import ManagerView from "../components/management/manager/ManagerView";

const ManagementRoutes = [
  { path: '/organization/edit/:id', element: <OrgEdit /> },

  { path: '/manager/list', element: <ManagerListView /> },
  { path: '/manager/:id', element: <ManagerView /> },
];

export default ManagementRoutes;
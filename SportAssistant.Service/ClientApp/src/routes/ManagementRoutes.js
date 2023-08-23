import OrgEdit from "../components/management/organization/OrgEdit";
import ManagerListView from "../components/management/manager/ManagerListView";
import ManagerView from "../components/management/manager/ManagerView";
import AssignedCoachListView from "../components/management/assignedCoach/AssignedCoachListView";
import AssignedCoachView from "../components/management/assignedCoach/AssignedCoachView";


const ManagementRoutes = [
  { path: '/organization/edit/:id', element: <OrgEdit /> },

  { path: '/manager/list', element: <ManagerListView /> },
  { path: '/manager/:id', element: <ManagerView /> },

  { path: '/assignedCoaches/list', element: <AssignedCoachListView /> },
  { path: '/assignedCoach/:id', element: <AssignedCoachView /> },
];

export default ManagementRoutes;
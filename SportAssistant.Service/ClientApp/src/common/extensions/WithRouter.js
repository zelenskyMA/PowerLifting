import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';

// Позволяет превращать классы в функциональные компоненты и юзать хуки в рамках них.
const withRouter = WrappedComponent => props => {
  const params = useParams();
  const navigate = useNavigate();

  return (
    <WrappedComponent
      {...props}
      params={params}
      navigate={navigate}
    />
  );
};

export default withRouter;
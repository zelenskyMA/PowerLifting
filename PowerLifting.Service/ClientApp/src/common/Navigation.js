import React from "react";
import { useNavigate } from "react-router-dom";
import { Button } from 'reactstrap';

export function GoToButton({ url, beforeNavigate, name = "Подтвердить" }) {
  let navigate = useNavigate();

  return (
    <Button onClick={async (event) => {
      await beforeNavigate();
      navigate(url);
    }}>
      {name}
    </Button>
  );
}
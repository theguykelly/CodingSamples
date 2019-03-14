import React from "react";
import "./influencer.css";
import PropTypes from "prop-types";
import { Button } from "reactstrap";
import { withRouter } from "react-router-dom";
import logger from "../../logger";

const _logger = logger.extend("InfluencersPaginated");

class InfluencersPaginated extends React.Component {
  inviteInfluencer = influencerId => {
    _logger(influencerId);
  };

  getInfluencerId = id => {
    this.props.getInfluencerId(id);
  };

  getInfluencerEmail = userId => {
    this.props.getInfluencerEmail(userId);
  };

  push = userId => {
    window.location.assign(`/instagram/profile?userId=${userId}`);
  };

  onInvitationClick = (id, userId) => {
    this.getInfluencerId(id);
    this.getInfluencerEmail(userId);
  };

  mappedList = data => {
    _logger(`mapped list`, data);
    const influencersList = this.props.get10.map(data => (
      <tr key={data.id} className="table">
        <td>
          <button
            type="button"
            className="btn btn-success"
            onClick={
              this.props.history.location.pathname === "/influencers"
                ? () => {
                    this.props.history.push("/campaigns/myCampaigns");
                  }
                : () => this.onInvitationClick(data.id, data.userId)
            }
          >
            Invite
          </button>
        </td>
        <td>
          <a
            href="#"
            onClick={e => {
              e.preventDefault();
              this.push(data.userId);
            }}
          >
            {
              <img
                src="https://instagram-brand.com/wp-content/uploads/2016/11/Instagram_AppIcon_Aug2017.png?w=30"
                alt=""
              />
            }
          </a>
        </td>
        <td>
          <a
            href="#"
            onClick={e => {
              e.preventDefault();
              this.push(data.userId);
            }}
          >
            {data.instagramUsername
              ? `@${data.instagramUsername}`
              : "@KellyKaliente"}
          </a>
        </td>
        <td>
          {data.firstName} {data.lastName}
        </td>
        <td>{data.instagramBio ? data.instagramBio : data.bio}</td>
        <td>
          {data.instagramUsername
            ? `${data.instagramFollowers} Followers`
            : "15k Followers"}
        </td>
        <td>45 Milestones</td>
        <td>2 Active</td>
        <td>
          <Button
            color="light"
            size="sm"
            onClick={() => this.getFaqByUserIdPushPage(data.userId, 2)}
          >
            View
          </Button>
        </td>
      </tr>
    ));
    return influencersList;
  };

  getFaqByUserIdPushPage = userId => {
    this.props.history.push("/faqs/influencer/" + userId);
  };

  render() {
    return (
      <React.Fragment>
        <div className="col-md-12">
          <div className="card card-outline-info h-100">
            <h3 className="card-header text-white">Influencer Marketplace</h3>
            <div className="d-flex no-block">
              <div className="col-md-12">
                <div className="table table-responsive stylish-table">
                  <table className="col-md-12 table-bordered table-hover footable-5 footable-paging footable-paging-center breakpoint-lg">
                    <thead>
                      <tr>
                        <th>Invite</th>
                        <th>IG</th>
                        <th>Username</th>
                        <th>Name</th>
                        <th>Bio</th>
                        <th>Followers</th>
                        <th>Stats</th>
                        <th>Campaigns</th>
                        <th>FAQs</th>
                      </tr>
                    </thead>
                    <tbody>
                      {this.props.get10 && this.mappedList(this.props.get10)}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

InfluencersPaginated.propTypes = {
  get10: PropTypes.array,
  getInfluencerId: PropTypes.func,
  history: PropTypes.object,
  getInfluencerEmail: PropTypes.func
};

export default withRouter(InfluencersPaginated);
